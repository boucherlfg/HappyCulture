using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StatDisplay : MonoBehaviour
{
    public TMP_Text label;
    public Stats.Name statName;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => Stats.Instance);

        Stats.Instance.Changed += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        label.text = "" + Stats.Instance[statName];
    }
}
