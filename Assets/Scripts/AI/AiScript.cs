using Unity.Burst.CompilerServices;
using UnityEngine;

public class Targets : MonoBehaviour
{
    [SerializeField] private float hp = 100f;
    public float damageCount = 10;
    [SerializeField] private Transform PlayerTargets;
    private Collider m_Collider;
    private Animator zomb;

    private AudioSource Sound;
    [SerializeField] private AudioClip hitSound;
    private float amount2;
    [SerializeField] private GameObject ammo;



    //[SerializeField] private float delay;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        zomb = GetComponent<Animator>();
        Sound = GetComponent<AudioSource>();
        // playermanager.damageCount = damageCount;
    }

    private void OnEnable()
    {
      //  playermanager.damageDelay = delay;
    }

    public void TakeDamage(float amount)
    {
        

        if(GameObject.Find("axeArms") != null)
        {
            amount2 = amount;
            Invoke("hitdelay", 0.3f);
            
        }
        else
        {
            run.speed = 4;
            hp -= amount;
            Sound.PlayOneShot(hitSound, 1F);
            if (hp <= 0f)
            {
                zomb.SetBool("death", true);
                var z = Random.Range(-1, 3);
                Debug.Log(z);
                if (z == -1)
                {
                    Instantiate(ammo, new Vector3(transform.position.x, -27.521f, transform.position.z), Quaternion.identity);
                }
                Die();
            }

            else
            {

                //    zomb.Play("Base Layer.hit", 0);

                //   Invoke("HitComplete", 0.7f);
            }
        }

    }

    private void Die() 
    {
        GetComponent<Targets>().enabled = false;
        m_Collider.enabled = !m_Collider.enabled;

        Destroy(gameObject, 10f);
    }

    private void HitComplete()
    {
        zomb.Play("Base Layer.run", 0);
    }
    private void hitdelay()
    {
        Sound.PlayOneShot(hitSound, 1F);
        run.speed = 4;
        hp -= amount2;
        if (hp <= 0f)
        {
            zomb.SetBool("death", true);
            Die();
            var z = Random.Range(-1, 0);
            Debug.Log(z);
            if (z == -1 && GameObject.Find("obj228") != null)
            {
                Instantiate(ammo, new Vector3(transform.position.x, -27.521f, transform.position.z), Quaternion.identity);
            }
        }
    }



}
