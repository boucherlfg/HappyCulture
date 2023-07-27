using System.Collections.Generic;
using UnityEngine;

public class Antenna : Buyable
{
    [SerializeField]
    private float range = 5;
    [SerializeField]
    private float speedBoost = 2;

    void Update()
    {
        UpdateScale();
        foreach (var bee in Bee.all)
        {
            var dist = Vector2.Distance(bee.transform.position, transform.position);
            if (dist >= range) continue;
            bee.GetSpeedBoost(speedBoost);
            Debug.Log("boost given");
        }
    }
}