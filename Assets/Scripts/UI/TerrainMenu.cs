using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerrainMenu : MonoBehaviour
{
    public GameObject available;
    public GameObject unavailable;
    int price;
    public TMP_Text costLabel;
    public Button buyButton;
    void OnDisable()
    {
        Inventory.Instance.Changed -= Refresh;
    }
    void OnEnable()
    {
        StartCoroutine(WaitForInventory());
        IEnumerator WaitForInventory()
        {
            yield return new WaitUntil(() => Inventory.Instance);
            Inventory.Instance.Changed += Refresh;
            Refresh();
        }
    }
    void Refresh()
    {
        int mapUpgrades = Map.Instance.MetamapSize;
        //faire apparaitre la fenêtre de limite atteinte
        if (mapUpgrades > Map.Instance.maxUpgradeCount)
        {
            unavailable.SetActive(true);
            available.SetActive(false);
        }
        price = ((1 << (mapUpgrades - 1)) * 100) / 2;
        costLabel.text = "" + price;
        buyButton.interactable = price <= Inventory.Instance.Honey;
    }

    public void BuyTerrain()
    {
        Map.Instance.AddMap();
        Inventory.Instance.Honey -= price;
    }
}
