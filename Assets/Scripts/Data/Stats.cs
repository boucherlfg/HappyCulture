using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(Stats))]
public class StatsEditor : Editor
{
    private Stats.Name statName;
    private int value;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        statName = (Stats.Name)EditorGUILayout.EnumPopup(statName);
        value = EditorGUILayout.IntField(value);
        if (GUILayout.Button("Set"))
        {
            Stats.Instance[statName] = value;
        }
    }
}
#endif

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
        while (true)
        {
            yield return new WaitForSeconds(60);
            this[Name.HoneyPerMinute] = this[Name.TotalHoney] - lastTotal;
            lastTotal = this[Name.TotalHoney];
        }
    }
    public int this[Name name]
    {
        get => values[name];
        set
        {
            values[name] = Mathf.Min(value, 99999);
            Changed?.Invoke();
        }
    }
}