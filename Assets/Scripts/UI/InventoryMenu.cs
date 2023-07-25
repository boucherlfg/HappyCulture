using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    void OnDisable()
    {
        Inventory.Instance.Changed -= Refresh;
    }
    void OnEnable()
    {
        Inventory.Instance.Changed += Refresh;
        Refresh();
    }
    public GameObject menuItem;
    public void Refresh()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (Inventory.Instance.Count() <= 0) return;

        foreach (var item in Inventory.Instance.Distinct().OrderBy(x => x.name))
        {
            var instance = Instantiate(menuItem);
            var comp = instance.GetComponent<MenuItem>();
            comp.Prefab = item.gameObject;
            comp.Count = Inventory.Instance.Count(x => x == item);
            comp.Sprite = comp.Prefab.GetComponent<SpriteRenderer>().sprite;
            instance.transform.SetParent(transform);
            instance.transform.localScale = Vector3.one;
        }
    }
}
