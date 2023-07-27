using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ext
{
    public static void MinInsert<T>(this List<T> list, T element) where T : ICompared<T>
    {
        for (int i = 0; i < list.Count; i++)
        {
            var comp = list[i];
            if (comp.CompareTo(element) < 0) continue;
            list.Insert(i, element);
            return;
        }
        list.Insert(list.Count, element);
    }
    public static bool Approx(this float me, float other) => Mathf.Abs(me - other) < 0.001f;
    public static bool Approx(this Vector2 me, Vector2 other) => Vector2.Distance(me, other).Approx(0);
    public static bool Approx(this Vector3 me, Vector3 other) => Vector2.Distance(me, other).Approx(0);
    public static Vector2 RoundToMultiple(this Vector2 vector, float multiple)
    {
        vector.x = vector.x.RoundToMultiple(multiple);
        vector.y = vector.y.RoundToMultiple(multiple);
        return vector;
    }
    public static float RoundToMultiple(this float number, float multiple)
    {
        var result = Mathf.Abs(number) + multiple / 2f;
        result -= result % multiple;
        return result * Mathf.Sign(number);
    }
    public interface ICompared<T>
    {
        int CompareTo(T element);
    }
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