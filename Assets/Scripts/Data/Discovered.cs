using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class Discovered : MonoSingleton<Discovered> 
{
    private HashSet<string> items = new HashSet<string>();
    public bool this[string name] 
    {
        get => items.Contains(name);
    }
    public void Add(string name) => items.Add(name);
    IEnumerator Start()
    {
        yield return new WaitUntil(() => Inventory.Instance && Shop.Instance);
        Inventory.Instance.Changed += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        foreach (var item in Inventory.Instance.inventory.Distinct())
        {
            Add(item.name);
        }
        foreach (var item in Shop.Instance.itemsWithPrice.Distinct())
        {
            if (Inventory.Instance.Honey >= item.price)
            {
                Add(item.name);
            }
        }
    }
}