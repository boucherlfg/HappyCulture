using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Ext
{
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