using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ext
{
    public static Vector2 FindValidPosition(Vector2 origin, float radius)
    {
        var bounds = Map.Instance.SquareBound;
        Vector2 whereToSpawn;

        for (int _ = 0; _ < 1000; _++)
        {
            whereToSpawn = origin + Random.insideUnitCircle * radius;

            if (!Map.Instance.Contains(whereToSpawn)) continue;
            if (Physics2D.OverlapPoint(whereToSpawn)) continue;
            return whereToSpawn;
        }
        throw new System.Exception("cant find valid point");
    }
    public static Vector2 FindValidPosition()
    {
        var bounds = Map.Instance.SquareBound;
        Vector2 whereToSpawn;

        for (int _ = 0; _ < 1000; _++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            whereToSpawn = new Vector2(x, y);

            if (!Map.Instance.Contains(whereToSpawn)) continue;
            if (Physics2D.OverlapPoint(whereToSpawn)) continue;
            return whereToSpawn;
        }
        throw new System.Exception("cant find valid point");
    }
    public static T[] GetEnumAsArray<T>() where T : System.Enum
    {
        return System.Enum.GetValues(typeof(T)).Cast<T>().ToArray();
    }
    public static T PickRandom<T>(this IEnumerable<T> list)
    {
        var index = Random.Range(0, list.Count());
        var obj = list.ElementAt(index);
        return obj;
    }
    public static T Minimum<T>(this IEnumerable<T> list, System.Func<T, float> func)
    {
        if (list.Count() <= 0) return default;

        var most = list.ElementAt(0);
        var mostValue = func(most);
        foreach (var elem in list)
        {
            var value = func(elem);
            if (value < mostValue)
            {
                most = elem;
                mostValue = value;
            }
        }
        return most;
    }
}