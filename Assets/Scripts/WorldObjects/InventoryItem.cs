using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    private Collider2D col;
    private Camera mainCam;
    void Awake()
    {
        mainCam = Camera.main;
        col = GetComponent<Collider2D>();
    }
    public virtual bool OnClick()
    {
        return false;
    }
    public bool Hover 
    {
        get
        {
            var mouse = mainCam.ScreenToWorldPoint(Input.mousePosition);
            return col && col.OverlapPoint(mouse);
        }
    }
}