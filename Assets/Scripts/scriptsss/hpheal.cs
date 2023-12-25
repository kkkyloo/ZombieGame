
using System.Collections;
using UnityEngine;

public class hpheal : MonoBehaviour
{
    [SerializeField] private GameObject obj;


    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playermanager.Hpbar.fillAmount = 1;
            
            StartCoroutine(heal());

        }
    }

    IEnumerator heal()
    {
        obj.GetComponent<MeshRenderer>().enabled = false;
        obj.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(40);
        obj.GetComponent<MeshRenderer>().enabled = true;
        obj.GetComponent<Collider>().enabled = true;
    }



}
