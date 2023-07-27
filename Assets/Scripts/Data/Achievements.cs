using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoSingleton<Achievements>
{
    public event System.Action<Achievement> Changed;
    public List<Achievement> achievements;
    public List<Achievement> unlocked;
    IEnumerator Start()
    {
        yield return new WaitUntil(() => Stats.Instance);
        Stats.Instance.Changed += Instance_Changed;
    }

    private void Instance_Changed()
    {
        foreach (var ach in achievements)
        {
            if (unlocked.Contains(ach)) continue;
            var stat = Stats.Instance[ach.stat];
            if (stat >= ach.count)
            {
                unlocked.Add(ach);
                Changed?.Invoke(ach);
            }
        }
    }
}