using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public List<GameObject> trash;
    public float spawnInterval;
    private float spawnCounter;
    // Update is called once per frame
    void Update()
    {
        spawnCounter += Time.deltaTime;
        if (spawnCounter < spawnInterval) return;

        spawnCounter = 0;
        Spawn();
    }
    void Spawn()
    {
        Vector2 whereToSpawn = FindValidPosition();
        var toSpawn = trash.PickRandom();
        Instantiate(toSpawn, whereToSpawn, Quaternion.identity);
    }
    /// <summary>
    /// find a point that is in bounds and not over something else
    /// </summary>
    Vector2 FindValidPosition()
    {
        var bounds = Map.Instance.SquareBound;
        Vector2 whereToSpawn;
        
        for (int _ = 0; _ < 1000; _++)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            whereToSpawn = new Vector2(x, y);

            if (!Map.Instance.Contains(whereToSpawn)) continue;
            if (Physics2D.OverlapPoint(whereToSpawn)) continue;
            return whereToSpawn;
        }
        throw new System.Exception("cant find valid point");
    }
}
