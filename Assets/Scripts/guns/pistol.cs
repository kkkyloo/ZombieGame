using UnityEngine;
public class pistol : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackDelay = 0.3f;

    private Camera fpsCam;

    private ParticleSystem flash;

    private Animator animator;
    private string currentAnimaton;
    const string Pistol_IDLE = "Pistol_IDLE";
    const string Pistol_FIRE = "Pistol_FIRE";

    private bool attacking = false;

    private AudioSource pistolSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        fpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
        flash =  GameObject.Find("gunMuzzleFlash").GetComponent<ParticleSystem>();
        pistolSound = GetComponent<AudioSource>();
    }

    void Update()
    {   
        if (Input.GetButton("Fire1") && GameObject.FindGameObjectWithTag("pistol").activeSelf && !attacking) 
        {
            attacking = true;

            pistolSound.Play();

            Shoot();
        }
        
    }

    void Shoot()
    {
        ChangeAnimationState(Pistol_FIRE);
        
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit) && GameObject.FindGameObjectWithTag("pistol").activeSelf)
        {
            Targets targets = hit.transform.GetComponent<Targets>(); 

            if(targets != null)
            {
                targets.TakeDamage(damage);
            }   
        }

        flash.Play();

        
        Invoke("AttackComplete", attackDelay);
 
        ChangeAnimationState(Pistol_IDLE);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void AttackComplete()
    {
        attacking = false;
    }
}
