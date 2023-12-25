using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundsInit : MonoBehaviour
{
    public string volumeParameter = "MasterVolume";
    public AudioMixer mixer;


    void Start()
    {
        var volumeValue = PlayerPrefs.GetFloat(volumeParameter,1);
        mixer.SetFloat(volumeParameter, volumeValue);
    }

    
}
