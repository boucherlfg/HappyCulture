using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    public float zoomSpeed = 0.2f;
    public float dragSpeed = 0.1f;
    public float zoomMin = 1, zoomMax = 10;

    private Vector3 dragOrigin;
    private Camera mainCam;
    int lastButtonPressed;
    void Start()
    {
        mainCam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        var scrollDelta = Input.mouseScrollDelta;
        mainCam.orthographicSize -= scrollDelta.y * zoomSpeed;
        mainCam.orthographicSize = Mathf.Max(zoomMin, mainCam.orthographicSize);
        mainCam.orthographicSize = Mathf.Min(zoomMax, mainCam.orthographicSize);

        var right = Input.GetMouseButtonDown(1);
        var mid = Input.GetMouseButtonDown(2);
        if (lastButtonPressed < 0 && (mid || right))
        {
            if (right) lastButtonPressed = 1;
            else if (mid) lastButtonPressed = 2;

            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            lastButtonPressed = -1;
            return;
        }

        Vector2 pos = mainCam.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Vector2 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed);

        transform.Translate(move, Space.World);
    }
}
