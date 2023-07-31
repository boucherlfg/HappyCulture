using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuItem : MonoBehaviour, IPointerDownHandler
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
        PlaySound();
    }
    public void PlaySound()
    {
        Sound.Instance.PlayOnce(clickSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Place();
    }
}
