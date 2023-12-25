using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistolnewscript : MonoBehaviour
{
    [SerializeField] private float damage = 51f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float range = 100;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private AudioClip impact;

    private AudioSource Sound;

    private float startPosX;
    private float startPosY;

    private Camera fpsCam;

    private Animator animator;
    private string currentAnimaton;
    private string IDLE;
    private string FIRE;
    private string enableRUN;
    private string disableRUN;

    private bool isAttackPressed;
    private bool isAttacking;
    private bool isRunning;

    private GameObject player;

    private float secundomer;

    public static bool animComplite = true;

    private Vector3 position;
    private Quaternion rotation;

    private void OnEnable()
    {
        ChangeAnimationState(IDLE); 
       // transform.position = position;
      //  transform.rotation = rotation;
    }

    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;

        animator = GetComponent<Animator>();
        Sound = GetComponent<AudioSource>();
        fpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        IDLE = clips[0].name.ToString();
        
        enableRUN = clips[2].name.ToString();
        disableRUN = clips[1].name.ToString();
        FIRE = clips[4].name.ToString();
        player = GameObject.Find("Player"); 
    }



    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttackPressed = true;
            animComplite = false;
            startPosX = player.transform.position.x;
            startPosY = player.transform.position.y;
        }
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            isRunning = true;
            secundomer += Time.deltaTime;
        }
        else
        {
            startPosX = player.transform.position.x;
            startPosY = player.transform.position.y;
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        if (isAttackPressed)
        {
            flash.Play();
            isAttackPressed = false;

            if (!isAttacking)
            {
                isAttacking = true;

                ChangeAnimationState(FIRE);

                Invoke("AttackComplete", attackDelay);
                
                Sound.PlayOneShot(impact, 0.7F);

                RaycastHit hit;

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    Targets targets = hit.transform.GetComponent<Targets>();

                    if (targets != null)
                    {
                        targets.TakeDamage(damage);
                    }
                }

            }
        }
        else if (!isAttacking && !isRunning && player.transform.position.x == startPosX)
        {
            ChangeAnimationState(IDLE);
            secundomer = 0;
        }
        else if (isRunning && !isAttacking && secundomer > 0.15)
        {
            ChangeAnimationState(enableRUN);
        }
        else if (!isRunning && !isAttacking && player.transform.position.x != startPosX && secundomer > 0.2)
        {
            ChangeAnimationState(disableRUN);
        }

    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

    private void AttackComplete()
    {
        isAttacking = false;
        animComplite = true;
    }


}
