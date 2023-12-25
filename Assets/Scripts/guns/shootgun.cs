using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootgun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 10;

    public Camera fpsCam;

    public ParticleSystem flash;

    public GameObject ak;
    public GameObject sg;

    public AudioClip sg_clip;
    public AudioSource _audiosource;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && sg.activeSelf) 
        {
            Shoot(); 
        }
        if (Input.GetKeyDown("1"))
        {
            ak.SetActive(true); 
            sg.SetActive(false); 
        }
    }

    void Shoot()
    {
        // _audiosource.PlayOneShot(sg_clip);

        flash.Play();
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range) && sg.activeSelf)
        {
            Targets targets = hit.transform.GetComponent<Targets>(); 
            if(targets != null)
            {
                targets.TakeDamage(damage);
            }   
        }


    }
}
