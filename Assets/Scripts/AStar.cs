using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStar : Singleton<AStar>
{
    class Node : Ext.ICompared<Node>
    {
        public Vector2 pos;
        public float distanceTo;
        public float distanceFrom;
        public float metric => distanceFrom + distanceTo;
        public Node parent;
        public Node()
        {
            distanceFrom = float.MaxValue;
        }
        public Node(Vector2 pos, Node parent = null)
        {
            this.pos = pos;
            this.parent = parent;
        }
        public override string ToString()
        {
            return pos + " : " + metric;
        }
        public int CompareTo(Node other)
        {
            return Mathf.RoundToInt(Mathf.Sign(metric - other.metric));
        }
    }
    private List<Vector2> BuildPath(Node end)
    {
        List<Vector2> ret = new List<Vector2>();
        while (end.parent != null)
        {
            ret.Insert(0, end.pos);
            end = end.parent;
        }
        return ret;
    }

    public List<Vector2> GetPath(Vector2 start, Vector2 end, Func<Vector2, bool> isCollision = default, float grain = 1)
    {
        if (isCollision == default) isCollision = pos => Physics2D.OverlapPoint(pos);

        start = start.RoundToMultiple(grain);
        end = end.RoundToMultiple(grain);

        if (start == end) return new List<Vector2>();
        if (isCollision(end)) return new List<Vector2>();

        var notVisited = new List<Node>();
        var visited = new List<Node>();

        Node startNode = new Node(start);
        startNode.distanceTo = (end - start).sqrMagnitude;
        
        notVisited.MinInsert(startNode);

        for (int i = 0; i < 100000; i++)
        {
            if (notVisited.Count <= 0) break;

            var node = notVisited[0];
            notVisited.RemoveAt(0);
            if (node.pos == end)
                return BuildPath(node);

            visited.Add(node);

            AddNeighbours(node, notVisited, visited, end, grain, isCollision);
        }
        return new List<Vector2>();
    }
    void AddNeighbours(Node node, List<Node> notVisited, List<Node> visited, Vector2 end, float grain, Func<Vector2, bool> isCollision)
    {
        for (float i = -grain; i <= grain; i += grain)
        {
            for (float j = -grain; j <= grain; j += grain)
            {
                if (i.Approx(0) && j.Approx(0)) continue;

                var pos = new Vector2(node.pos.x + i, node.pos.y + j);

                if (visited.Exists(n => n.pos.Approx(pos))) continue;
                if(notVisited.Exists(n => n.pos.Approx(pos))) continue;
                if (isCollision(pos)) continue;

                var toAdd = new Node(pos, node);
                toAdd.distanceTo = (end - pos).sqrMagnitude;
                toAdd.distanceFrom = (node.pos - pos).sqrMagnitude + node.distanceFrom;
                notVisited.MinInsert(toAdd);
            }
        }
    }
}
