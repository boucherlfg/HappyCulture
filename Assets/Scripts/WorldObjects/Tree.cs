using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tree : Buyable
{
    public List<Flower> flowers;
    public float spawnInterval;
    private float spawnCounter;
    public float flowerSpawnRange;
    void Update()
    {
        SpawnFlowers();
        UpdateScale();
    }

    void SpawnFlowers()
    {
        spawnCounter += Time.deltaTime;
        if (spawnCounter < spawnInterval) return;
        spawnCounter = 0;

        var flowerPartition = flowers.Count() / 3;
        var flowerTiers = new IEnumerable<Flower>[]
        {
           flowers.Take(flowerPartition),
           flowers.Skip(flowerPartition).Take(flowerPartition),
           flowers.Skip(flowerPartition * 2).Take(flowerPartition),
        };
        int flowerIndex = (int)(Mathf.Pow(Random.value, Mathf.Exp(1)) * flowerTiers.Count());
        var flower = flowerTiers.ElementAt(flowerIndex).PickRandom();

        var pos = Ext.FindValidPosition(transform.position, flowerSpawnRange);
        var spawn = Instantiate(flower, pos, Quaternion.identity);
        spawn.transform.SetParent(transform.parent);
    }
}
