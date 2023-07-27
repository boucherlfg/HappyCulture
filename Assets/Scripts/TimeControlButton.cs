using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlButton : MonoBehaviour
{
    [SerializeField]
    float targetValue;
    public void Change()
    {
        if (Mathf.Approximately(targetValue, Timescale.Instance.Value))
        {
            Timescale.Instance.Value = 1;
            return;
        }
        Timescale.Instance.Value = targetValue;
    }
}
