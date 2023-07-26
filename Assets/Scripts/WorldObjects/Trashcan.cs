using UnityEngine;

public class Trashcan : InventoryItem
{
    void Update()
    {
        transform.localScale = Vector3.one;
        if (Hover)
        {
            transform.localScale *= 1.2f;
        }
    }
}