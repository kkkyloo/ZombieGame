using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounManagerButton : MonoBehaviour
{
    public AudioClip sound;
    public AudioSource a;
    public void ButtonSound()
    {
        a.clip = sound;
        a.Play();
    }
}
