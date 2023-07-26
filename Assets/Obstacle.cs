using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : InventoryItem
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
