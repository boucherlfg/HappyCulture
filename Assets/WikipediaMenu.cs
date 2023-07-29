using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WikipediaMenu : MonoBehaviour
{
    public void OnDisable()
    {
        Open(transform.GetChild(0).gameObject);
    }
    public void Open(GameObject toOpen)
    {
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(false);
        }
        toOpen.SetActive(true);
    }
    public void OpenNext()
    {
        int toOpen = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeSelf) toOpen = i + 1;
            child.gameObject.SetActive(false);
        }
        if (toOpen >= transform.childCount) toOpen = 0;
        transform.GetChild(toOpen).gameObject.SetActive(true);
    }
    public void OpenPrevious()
    {
        int toOpen = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child.gameObject.activeSelf) toOpen = i - 1;
            child.gameObject.SetActive(false);
        }
        if (toOpen < 0) toOpen = transform.childCount - 1;
        transform.GetChild(toOpen).gameObject.SetActive(true);
    }
}
