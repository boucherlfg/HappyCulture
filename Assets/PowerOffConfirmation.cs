using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOffConfirmation : MonoBehaviour
{
    public System.Action toDo;
    public void Confirm()
    {
        toDo?.Invoke();
    }
}
