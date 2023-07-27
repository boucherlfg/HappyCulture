using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text label;
    [SerializeField]
    private Image image;
    [SerializeField]
    private AudioClip clickSound;
    public GameObject Prefab
    {
        get;
        set;
    }
    public Sprite Sprite
    {
        get => image.sprite;
        set => image.sprite = value;
    }
    private int count;
    public int Count
    {
        get => count;
        set
        {
            label.text = "" + value;
            count = value;
        }
    }
    public void Place()
    {
        Inventory.Instance.Honey -= count;

        Shop.Instance.Count[Prefab.name]++;
        
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        MouseControl.Instance.Drag(Prefab, position);
    }
    public void PlaySound()
    {
        Sound.Instance.PlayOnce(clickSound);
    }
}
