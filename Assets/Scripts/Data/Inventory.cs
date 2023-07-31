using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoSingleton<Inventory>, IEnumerable<InventoryItem>
{
    [SerializeField]
    private int honey;
    public List<InventoryItem> inventory;
    public event System.Action Changed;

    public int Honey
    {
        get => honey;
        set
        {
            honey = value;
            
            Changed?.Invoke();
        }
    }
    public void Add(InventoryItem element)
    {
        inventory.Add(element);
        Changed?.Invoke();
    }
    public void Remove(InventoryItem element)
    {
        inventory.Remove(element);
        Changed?.Invoke();
    }
    public Inventory()
    {
        inventory = new List<InventoryItem>();
    }

    public IEnumerator<InventoryItem> GetEnumerator()
    {
        return inventory.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return inventory.GetEnumerator();
    }
}