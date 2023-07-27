using System.Collections.Generic;

public class Shop : MonoSingleton<Shop>
{
    public List<Buyable> itemsWithPrice;

    private StringIntDict count = new StringIntDict();
    public StringIntDict Count => count;

    public void Buyback(Buyable buyable)
    {
        //take back buy count
        Count[buyable.name]--;
        //give back honey to player
        Stats.Instance[Stats.Name.TotalHoney] -= buyable.Price;
        Inventory.Instance.Honey += buyable.Price;
    }
}