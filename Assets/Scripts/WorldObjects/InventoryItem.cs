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
    protected virtual void UpdateScale()
    {
        transform.localScale = Vector3.one;
        if (Hover)
        {
            transform.localScale *= 1.2f;
        }
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