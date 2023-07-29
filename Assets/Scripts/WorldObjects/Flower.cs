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

    protected virtual void Start()
    {
        Stats.Instance[Stats.Name.FlowersPlaced]++;
        Stats.Instance[Stats.Name.CurrentFlowers]++;
    }
    protected virtual void OnDestroy()
    {
        Stats.Instance[Stats.Name.CurrentFlowers]--;
    }
    void Update()
    {
        UpdateScale();
		if(lifetime <= 0) {
			Destroy(gameObject);
		}
    }
    public int TakePollen()
    {
        lifetime -= pollen;
        return pollen;
    }
}
