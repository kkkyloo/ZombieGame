using Unity.Burst.CompilerServices;
using UnityEngine;

public class gunsScript : MonoBehaviour
{
    [SerializeField]
    private float damage = 51f;
    [SerializeField] private float attackDelay = 1.8f;

    private string currentAnimaton;
    private Animator animator;

    private string IDLE;
    private string FIRE;
    //private string FIRE2;

    private bool isAttackPressed; 
    private bool isAttacking;

    [SerializeField] private AudioClip impact;

    private AudioSource Sound;
    [SerializeField] private GameObject prefab; 
    bool Issound = true;

    void Awake()
    {
        animator = GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        IDLE = clips[0].name.ToString();
        FIRE = clips[1].name.ToString();
        //FIRE2 = clips[2].name.ToString();
        Sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ChangeAnimationState(IDLE); 
    }

    void Update()
    {
        if (PlayerCam.fire == true && GameObject.FindGameObjectWithTag("axeArms").activeSelf && this.animator.GetCurrentAnimatorStateInfo(0).IsName("AxeIDLE"))
        {
            isAttackPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (isAttackPressed )
        {
            isAttackPressed = false;
            if(Issound)
            {
                Sound.PlayOneShot(impact, 0.7F);
                Issound = false;
            }
                

            if (!isAttacking)
            {
                isAttacking = true;
                //int x = Random.Range(-1, 1);
                //if (x == 0)
                //{ 
                ChangeAnimationState(FIRE);
                    //attackDelay = 1.8f;
                    //damage = 60f;
                GetComponent<Collider>().enabled = true;

                //}
                //if (x == -1)
                //{
                //    ChangeAnimationState(FIRE2);
                //    attackDelay = 1f;
                //     damage = 40f;
                // }

                //attackDelay = animator.GetCurrentAnimatorStateInfo(0).length;
                Invoke("AttackComplete", attackDelay);
                Invoke("DamageTrigger", 0.8f);


            }
        }
        if (!isAttacking)
            ChangeAnimationState(IDLE);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void DamageTrigger()
    {
        GetComponent<Collider>().enabled = false;
    }
    void AttackComplete()
    {
        Issound = true;
        isAttacking = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie")
        {
            Destroy(Instantiate(prefab, other.transform.position, Quaternion.identity), 0.5f);
            other.gameObject.GetComponent<Targets>().TakeDamage(damage);
        }
   
    }

    private void DamageSound()
    {
        
    }
}
