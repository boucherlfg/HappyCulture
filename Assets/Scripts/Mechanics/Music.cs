using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public void SetSound(float volume)
    {
        AudioListener.volume = volume;
    }
    public static void PlayOnce(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, Vector2.zero, 0.5f);
    }
}
