using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoSingleton<MouseControl>
{
    [SerializeField]
    private Transform garden;
    [SerializeField]
    private float dragSpeed = 20;
    [SerializeField]
    private AudioClip trashSound;

    private Camera mainCam;
    private GameObject dragged;
    private bool fromInventory = false;

    private bool IsClicking(int button)
    {
        var ret = Input.GetMouseButtonUp(button);
        ret &= !EventSystem.current.IsPointerOverGameObject();
        return ret;
    }
    private Collider2D GetOverlapping(Vector2 pos, GameObject gameObject)
    {
        var hits = Physics2D.OverlapPointAll(pos);
        return hits.FirstOrDefault(x => x.gameObject != gameObject);
    }
    public void Drag(GameObject toDrag, Vector2 position)
    {
        fromInventory = true;
        dragged = Instantiate(toDrag, position, Quaternion.identity);
        dragged.name = toDrag.name;
        ToggleObject(dragged);
    }

    void ToggleObject(GameObject obj)
    {
        obj.GetComponentsInChildren<MonoBehaviour>().ToList().ForEach(x => x.enabled = !x.enabled);
        obj.GetComponentsInChildren<Collider2D>().ToList().ForEach(x => x.enabled = !x.enabled);
    }
    void Start()
    {
        mainCam = Camera.main;
    }

    void UpdateDraggingInfo()
    {
        Vector2 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dragPos = dragged.transform.position;
        var diff = pos - dragPos;
        dragged.transform.position = Vector2.Lerp(dragPos, pos, Time.deltaTime * dragSpeed);

        var overlapping = GetOverlapping(pos, dragged);
        var trashcan = Physics2D.OverlapPointAll(pos).FirstOrDefault(x => x.GetComponent<Trashcan>());
        var canPlace = Map.Instance.Contains(pos) && !overlapping;

        var rend = dragged.GetComponentsInChildren<SpriteRenderer>().ToList();
        rend.ForEach(r => r.color = canPlace ? Color.white : Color.red);

        if (IsClicking(0))
        {
            if (canPlace)
            {
                fromInventory = false;
                dragged.transform.SetParent(garden);
                ToggleObject(dragged);
                dragged = null;
            }
            else if(trashcan)
            {
                if (fromInventory)
                {
                    Inventory.Instance.Add(Database.Instance.items.Find(x => x.name == dragged.name));
                    fromInventory = false;
                }
                Destroy(dragged);
                dragged = null;
                AudioSource.PlayClipAtPoint(trashSound, trashcan.transform.position, 1);
                
            }
        }
    }
    void StartDragging()
    {
        Vector2 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        if (IsClicking(0))
        {
            var hit = Physics2D.OverlapPointAll(pos).FirstOrDefault(x => x.gameObject != gameObject);

            if (hit)
            {
                var item = hit.GetComponent<InventoryItem>();
                if (!item || !item.OnClick())
                {
                    dragged = hit.gameObject;
                    ToggleObject(dragged);
                }
            }
        }
    }
    public void Update()
    {
        
        if (dragged)
        {
            UpdateDraggingInfo();
        }
        else
        {
            StartDragging();
        }
    }
}
