using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Buyable
{
    [SerializeField]
    private int pollen = 1;
    [SerializeField]
    public float lifetime = 3 * 60;
    [SerializeField]
    private FlowerType flowerType;
    public virtual bool IsFlowerOk(FlowerType toCheck) => flowerType == toCheck;
    // Update is called once per frame

    void Start()
    {
        Stats.Instance[Stats.Name.FlowersPlaced]++;
    }
    void Update()
    {
        if (lifetime < pollen) Destroy(gameObject);
    }
    public int TakePollen()
    {
        lifetime -= pollen;
        return pollen;
    }
}
