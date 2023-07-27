using UnityEngine;

/// <summary>
/// game over == no hives left, not enough money to buy new hives
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverLabel;
    void Start()
    {
        Stats.Instance.Changed += CheckForGameOver;
    }
    void OnDestroy()
    {
        Stats.Instance.Changed -= CheckForGameOver;
    }
    private void CheckForGameOver()
    {
        //no hives left
        bool noHivesLeft = Stats.Instance[Stats.Name.CurrentHive] <= 0;
        //can't buy new hives
        bool cannotBuyHives = Shop.Instance.itemsWithPrice.FindAll(x => x.GetComponent<BeeHive>())
                                                          .TrueForAll(x => x.Price > Inventory.Instance.Honey);

        gameOverLabel.SetActive(noHivesLeft && cannotBuyHives);
    }

}