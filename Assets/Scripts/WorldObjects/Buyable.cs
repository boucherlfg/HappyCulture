using System.Collections.Generic;
using UnityEngine;

public class Buyable : InventoryItem
{
    [System.Serializable]
    public struct UnlockCondition 
    {
        public Stats.Name name;
        public float qty;
    }
    public int price;
    public List<UnlockCondition> unlockConditions;
}