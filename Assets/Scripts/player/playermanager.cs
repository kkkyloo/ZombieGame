using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class playermanager : MonoBehaviour
{
    [SerializeField] public static int playerHealth = 100;
    [SerializeField] private TextMeshProUGUI playerHealthText;

    public static Image Hpbar;
    private float fill = 1f;

    //  [SerializeField] private float nextFire2 = 0f;
    // [SerializeField] private float fireRate2 = 1f;

    private Animator animator;
    
    private bool axeCollider;

    public static float damageCount;
  //  private string damage;

   // public static float damageDelay;

    private void Start()
    {
        animator = GameObject.Find("2").GetComponent<Animator>();
        Hpbar = GameObject.Find("hpbar2").GetComponent<Image>(); 


    }

   /* private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Zombi")
        {
            if (Time.time > nextFire2)
            {               
                Invoke("Damage", damageDelay);
                Invoke("DamageComplete", 1.5f);

                nextFire2 = Time.time + 1.5f; 

            }

            if (playerHealth <= 0)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                SceneManager.LoadScene("menu");
            }
        } 
        
    }*/

    private void DamageComplete()
    {
        animator.Play("idledamage");
    }


    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.tag == "arm")  
        {
            if (other.transform.root.GetComponent<Targets>().enabled)
            {
                if (GameObject.Find("axeArms") != null)
                {
                    if(!GameObject.FindWithTag("axeArms").GetComponent<Collider>().enabled)
                    {
                        damageCount = other.transform.root.GetComponent<Targets>().damageCount;

                        Hpbar.fillAmount -= damageCount;
                        animator.Play("damage");
                        Invoke("DamageComplete", 1f);
                    }

                }
                else
                {
                    damageCount = other.transform.root.GetComponent<Targets>().damageCount;

                    Hpbar.fillAmount -= damageCount;
                    animator.Play("damage");
                    Invoke("DamageComplete", 1f);
                }


            }



            if (Hpbar.fillAmount <= 0)
            {

                SceneManager.LoadScene("menu");
            }
        }

    }

}

