using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoneyDisplay : MonoBehaviour
{
    public TMP_Text label;

    void OnEnable()
    {
        Inventory.Instance.Changed += Refresh;
        Refresh();
    }
    void OnDisable()
    {
        Inventory.Instance.Changed -= Refresh;
    }
    void Refresh()
    {
        label.text = "" + Inventory.Instance.Honey;
    }
}
