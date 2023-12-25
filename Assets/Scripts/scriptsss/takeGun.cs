using UnityEngine;
using UnityEngine.UI;

public class takeGun : MonoBehaviour
{
    [SerializeField] private GameObject ak;
    [SerializeField] private GameObject shotgun;

    [SerializeField] private GameObject realak;
    [SerializeField] private GameObject realshotgun;

    [SerializeField] private GameObject obj1;
    [SerializeField] private GameObject obj2;
    private bool aktake = false;

    void Start()
    {
        ak.GetComponent<Button>().interactable = false;
        shotgun.GetComponent<Button>().interactable = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !ak.GetComponent<Button>().interactable && realak.activeSelf && !aktake)
        {
            ak.GetComponent<Button>().interactable = true;
            obj1.SetActive(true);
            
            aktake = true;
            Destroy(realak);

        }
         if (other.gameObject.tag == "Player" && realshotgun.activeSelf)
        {
            shotgun.GetComponent<Button>().interactable = true;
            obj2.SetActive(true);
            Destroy(realshotgun);
        }



    }




}
