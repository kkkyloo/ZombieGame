using UnityEngine;

public class axeScript : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float attackDelay = 0.3f;

    [SerializeField]
    private float range = 3;

    private Camera fpsCam;
    private string currentAnimaton;
    private Animator animator;

    const string AxeIDLE = "AxeIDLE";
    const string AxeFIRE = "AxeFIRE";

    private bool isAttackPressed;
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        fpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GameObject.FindGameObjectWithTag("axeArms").activeSelf)
        {
            isAttackPressed = true;

        }
    }

    private void FixedUpdate()
    {
        if (isAttackPressed)
        {
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;
                ChangeAnimationState(AxeFIRE);

                attackDelay = animator.GetCurrentAnimatorStateInfo(0).length - 0.2f;
                Invoke("AttackComplete", attackDelay);

                RaycastHit hit;

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range) && GameObject.FindGameObjectWithTag("axeArms").activeSelf)
                {
                    Targets targets = hit.transform.GetComponent<Targets>();

                    if (targets != null)
                    {
                        targets.TakeDamage(damage);
                    }
                }
            }
        }
        if (!isAttacking)
            ChangeAnimationState(AxeIDLE);
    }

    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    void AttackComplete()
    {
        isAttacking = false;
    }
}
