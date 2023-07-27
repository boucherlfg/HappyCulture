using System.Collections;
using UnityEngine;

public class Trash : Flower
{
    public bool BadHoney => true;
    IEnumerable Start()
    {
        var rend = GetComponent<SpriteRenderer>();
        var color = rend.color;
        color.a = 0;
        rend.color = color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime;
            rend.color = color;
            yield return null;
        }
        rend.color = new Color(1, 1, 1, 1);
    }
    void Update()
    {
        transform.localScale = Vector3.one;
        if (Hover)
        {
            transform.localScale *= 1.2f;
        }
    }
    public override bool IsFlowerOk(FlowerType toCheck)
    {
        return true;
    }
    protected override void OnDestroy()
    {
        Stats.Instance[Stats.Name.TrashPicked]++;
    }
}