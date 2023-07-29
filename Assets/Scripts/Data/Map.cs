using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoSingleton<Map>
{
    public int MetamapSize => metaMap.Count;
    private const int terrain_size = 8;
    private List<Vector2Int> metaMap = new List<Vector2Int>();
    [SerializeField] private Tilemap tilemap;

    public RuleTile tile;
    public Rect Bounds(Vector2 pos) 
    {
        Rect rect = new Rect(pos, Vector2.zero);
        
        //left
        var delta = Vector2.zero;
        while (tilemap.GetTile(Vector3Int.RoundToInt(pos + delta + Vector2.left))) delta += Vector2.left;
        rect.xMin = pos.x + delta.x;

        //right
        delta = Vector2Int.zero;
        while (tilemap.GetTile(Vector3Int.RoundToInt(pos + delta + Vector2.right))) delta += Vector2.right;
        rect.xMax = pos.x + delta.x;

        //up
        delta = Vector2Int.zero;
        while (tilemap.GetTile(Vector3Int.RoundToInt(pos + delta + Vector2.up))) delta += Vector2.up;
        rect.yMax = pos.y + delta.y;

        //down
        delta = Vector2.zero;
        while (tilemap.GetTile(Vector3Int.RoundToInt(pos + delta + Vector2.down))) delta += Vector2Int.down;
        rect.yMin = pos.y + delta.y;

        return rect;
    }
    public Bounds SquareBound => tilemap.localBounds;
    public bool Contains(Vector3 pos)
    {
        return tilemap.GetTile(Vector3Int.RoundToInt(pos));
    }
    void Start()
    {
        metaMap.Add(Vector2Int.zero);
        Refresh();
    }
    void Refresh()
    {
        tilemap.ClearAllTiles();
        foreach (var metaPosition in metaMap)
        {
            Vector2Int pos = metaPosition * terrain_size;
            for (int i = pos.x - terrain_size / 2; i < pos.x + terrain_size / 2; i++)
            {
                for (int j = pos.y - terrain_size / 2; j < pos.y + terrain_size / 2; j++)
                {
                    tilemap.SetTile(new Vector3Int(i, j), tile);
                }
            }
        }
        tilemap.CompressBounds();
    }

    public void AddMap()
    {
        for (int _ = 0; _ < 200; _++) //infinite loop protection
        {
            var next = metaMap.PickRandom();

            var availableNeighbours = new List<Vector2Int>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i * i + j * j != 1) continue;

                    var x = i + next.x;
                    var y = j + next.y;

                    if(metaMap.Exists(u => u.x == x && u.y == y)) continue;
                    availableNeighbours.Add(new Vector2Int(x, y));
                }
            }

            if (availableNeighbours.Count() <= 0) continue;

            var pos = availableNeighbours.PickRandom();
            metaMap.Add(pos);

            Refresh();
            return;
        }

        Debug.LogWarning("no available terrain to buy");
    }

}