using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 1;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
