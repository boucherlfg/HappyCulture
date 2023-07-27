using UnityEngine;

public class Timescale : Singleton<Timescale>
{
    public event System.Action Changed;
    public float Value
    {
        get
        {
            return Time.timeScale;
        }
        set
        {
            Time.timeScale = value;
            Changed?.Invoke();
        }
    }
}