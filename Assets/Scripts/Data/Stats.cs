using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stats : MonoSingleton<Stats>
{
    public enum Name
    {
        HivesPlaced = 0, 
        FlowersPlaced = 1, 
        HivesDestroyed = 2, 
        BeesSpawned = 3, 
        TotalHoney = 4, 
        HoneyPerMinute = 5, 
        HiveDamage = 6, 
        TrashPicked = 7,
        CurrentHive = 8,
        CurrentBees = 9,
        CurrentFlowers = 10
    }
    public event System.Action Changed;

    private Dictionary<Name, int> values;
    public Stats()
    {
        values = new Dictionary<Name, int>();

        foreach (var name in Ext.GetEnumAsArray<Name>())
        {
            values[name] = 0;
        }
    }

    IEnumerator Start()
    {
        int lastTotal = this[Name.TotalHoney];
        int[] honeywins = new int[60];
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            i++;
            i %= 60;
            int newTotal = this[Name.TotalHoney];
            int diff = newTotal - lastTotal;
            lastTotal = newTotal;
            if (diff >= 0) honeywins[i] = diff;
            this[Name.HoneyPerMinute] = honeywins.Sum() / 60;
        }
    }
    public int this[Name name]
    {
        get => values[name];
        set
        {
            values[name] = value;
            Changed?.Invoke();
        }
    }
}