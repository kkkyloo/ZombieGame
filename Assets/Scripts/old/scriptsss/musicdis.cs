using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicdis : MonoBehaviour
{
    public bool musicOn = true;
    public bool soundOn = true;

    public AudioSource music;
    public float mVol;
    public AudioSource akSource;
    public float akS;
    public AudioSource sgSource;
    public float sgkS;
    public AudioSource axeSource;
    public float axegkS;
    public AudioSource walk;
    public float walks;



    void Start()
    {
        akS = akSource.volume;
        sgkS = akSource.volume;
        axegkS = akSource.volume;
        walks = walk.volume;

    }

    
    public void onMusic()
    {
        if (musicOn)
        {
            musicOn = false;
            disableMusic();
        }
        else
        {
            music.volume = mVol;
        }
    }
    public void onSound()
    {
        if (soundOn)
        {
            soundOn = false;
            disableSound();
        }
        else
        {
            akSource.volume = akS;
            sgSource.volume = sgkS;
            axeSource.volume = axegkS;
            walk.volume = walks;
        }
    }

    private void disableMusic()
    {

        music.volume = 0;
    }
    private void disableSound()
    {
        akSource.volume = 0;
        sgSource.volume = 0;
        axeSource.volume = 0;
        walk.volume = 0;
    }


}
