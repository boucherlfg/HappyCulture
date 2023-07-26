using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoSingleton<Sound>
{
    public AudioSource sfx;
    public AudioSource music;
    
    public void SetSound(float volume)
    {
        AudioListener.volume = volume;
    }
    public void PlayOnce(AudioClip clip, float volume = 1)
    {
        sfx.PlayOneShot(clip, volume);
    }
}
