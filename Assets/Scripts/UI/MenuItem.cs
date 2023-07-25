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
    public void Buy()
    {
        Inventory.Instance.Add(Prefab.GetComponent<InventoryItem>());
        Inventory.Instance.Honey -= count;
    }
    public void Place()
    {
        Inventory.Instance.Remove(Prefab.GetComponent<InventoryItem>());
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MouseControl.Instance.Drag(Prefab, position);
    }
    public void PlaySound()
    {
        var sound = Camera.main.GetComponentInChildren<AudioSource>();
        sound.PlayOneShot(clickSound);
    }
}
