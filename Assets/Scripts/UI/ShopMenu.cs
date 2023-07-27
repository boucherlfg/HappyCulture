using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    void OnDisable()
    {
        Shop.Instance.Count.Changed -= Refresh;
        Inventory.Instance.Changed -= Refresh;
    }
    void OnEnable()
    {
        Shop.Instance.Count.Changed += Refresh;
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

        foreach (var item in Shop.Instance.itemsWithPrice)
        {
            if (!Discovered.Instance[item.name]) continue;
            int price = item.Price;
            var instance = Instantiate(menuItem);
            var comp = instance.GetComponent<MenuItem>();
            comp.Prefab = item.gameObject;
            comp.Count = price;
            comp.Sprite = comp.Prefab.GetComponent<SpriteRenderer>().sprite;

            var button = instance.GetComponent<Button>();
            button.interactable = Inventory.Instance.Honey >= price;
            instance.transform.SetParent(transform); 
            instance.transform.localScale = Vector3.one;
        }
    }
}
