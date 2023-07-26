using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoSingleton<Settings>
{
    public event System.Action Changed;
    private bool peaceMode;
    public bool PeaceMode
    {
        get => peaceMode;
        set
        {
            peaceMode = value;
            Changed?.Invoke();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
