using System.Collections.Generic;
using UnityEngine;

public class Buyable : InventoryItem
{
    [SerializeField]
    private bool freeOnFirstBuy;

    [System.Serializable]
    public struct UnlockCondition 
    {
        public Stats.Name name;
        public float qty;
    }
    /// <summary>
    /// item might be free on first buy
    /// </summary>
    public int Price => freeOnFirstBuy && Shop.Instance.Count[name] <= 0 ? 0 : price;
    [SerializeField]
    private int price;
    public List<UnlockCondition> unlockConditions;
}