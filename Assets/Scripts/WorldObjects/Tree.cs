using System.Collections;
using System.Collections.Generic;
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

        int value = (int)(Mathf.Pow(Random.value, Mathf.Exp(1)) * flowers.Count);
        var pos = Ext.FindValidPosition(transform.position, flowerSpawnRange);
        var spawn = Instantiate(flowers[value], pos, Quaternion.identity);
        spawn.transform.SetParent(transform.parent);
    }
}
