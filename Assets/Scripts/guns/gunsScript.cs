using UnityEngine;

public class GunsScript : MonoBehaviour
{
    [SerializeField]
    private float damage = 51f;
    [SerializeField]
    private float attackDelay = 1.8f;

    private string currentAnimation;
    private Animator animator;

    private string idleAnimation;
    private string fireAnimation;

    private bool isAttackPressed;
    private bool isAttacking;

    [SerializeField]
    private AudioClip impact;

    private AudioSource sound;
    [SerializeField]
    private GameObject prefab;
    private bool isSoundEnabled = true;

    void Awake()
    {
        animator = GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        idleAnimation = clips[0].name;
        fireAnimation = clips[1].name;

        sound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ChangeAnimationState(idleAnimation);
    }

    void Update()
    {
        if (PlayerCam.fire && GameObject.FindGameObjectWithTag("axeArms").activeSelf &&
            animator.GetCurrentAnimatorStateInfo(0).IsName("AxeIDLE"))
        {
            isAttackPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (isSoundEnabled)
            {
                sound.PlayOneShot(impact, 0.7F);
                isSoundEnabled = false;
            }

            if (!isAttacking)
            {
                isAttacking = true;
                ChangeAnimationState(fireAnimation);
                GetComponent<Collider>().enabled = true;

                Invoke("AttackComplete", attackDelay);
                Invoke("DamageTrigger", 0.8f);
            }
        }

        if (!isAttacking)
            ChangeAnimationState(idleAnimation);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    void DamageTrigger()
    {
        GetComponent<Collider>().enabled = false;
    }

    void AttackComplete()
    {
        isSoundEnabled = true;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Destroy(Instantiate(prefab, other.transform.position, Quaternion.identity), 0.5f);
            other.gameObject.GetComponent<Targets>().TakeDamage(damage);
        }
    }

    private void DamageSound()
    {
        // Implement if needed
    }
}
