using System.Collections;
using UnityEngine;

public class Trash : Flower
{
    public bool BadHoney => true;

    public override bool IsFlowerOk(FlowerType toCheck)
    {
        return true;
    }
    protected override void OnDestroy()
    {
        Stats.Instance[Stats.Name.TrashPicked]++;
    }
}