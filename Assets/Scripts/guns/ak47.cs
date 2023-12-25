using TMPro;
using UnityEngine;
using System.Collections;

public class Ak47 : MonoBehaviour
{
    [SerializeField] private float damage = 51f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float range = 20;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip noAmmo;

    [SerializeField] public static int MaxAmmo = 60;
    [SerializeField] public static int currentAmmo = 30;

    private AudioSource akSound;

    private float startPosX;
    private float startPosY;

    private Camera fpsCam;

    public static Animator animator;
    private string currentAnimaton;
    public static string IDLE;
    private string FIRE;
    private string enableRUN;
    private string disableRUN;
    private string reloadAnim;

    private bool isAttackPressed;
    private bool isAttacking;
    private bool isRunning;

    private GameObject player;

    private float secundomer;
    public static bool animComplite = true;

    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private GameObject _prefab;

    [SerializeField] public static TextMeshProUGUI ammoText;

    public static bool isReloading = false;




    private void Start()
    {
        animator = GetComponent<Animator>();
        akSound = GetComponent<AudioSource>();
        fpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        IDLE = clips[0].name.ToString();
        FIRE = clips[1].name.ToString();
        enableRUN = clips[3].name.ToString();
        disableRUN = clips[4].name.ToString();

        reloadAnim = clips[5].name.ToString();
        ammoText = GameObject.Find("ammoAk").GetComponent<TextMeshProUGUI>();

        player = GameObject.Find("Player");

    }



    private void OnEnable()
    {
        ChangeAnimationState(IDLE); 
    }

    private void Update()
    {
        if (isReloading)
        {
            return;
        }


        if (currentAmmo == 0 && MaxAmmo > 0 && currentAmmo != 30) 
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
            return;
        }


        if (PlayerCam.fire == true && currentAmmo > 0)
        {
            isAttackPressed = true;
            startPosX = player.transform.position.x;
            startPosY = player.transform.position.y;
            animComplite = false;
        }



        if (Mathf.Abs(PlayerMovement.horizontalInput) > 0 || Mathf.Abs(PlayerMovement.verticalInput) > 0)
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




    IEnumerator ReloadGun()
    {
        akSound.PlayOneShot(reload, 1F);
        ChangeAnimationState(reloadAnim);

        yield return new WaitForSeconds(2);


        int amountToWithdraw = Mathf.Min(30 - currentAmmo, MaxAmmo);
        MaxAmmo -= amountToWithdraw;
        currentAmmo += amountToWithdraw;


        ammoText.text = currentAmmo + " / " + MaxAmmo;

        isReloading = false;
        ChangeAnimationState(IDLE);
    }

    public void ReloadDown()
    {
        if (MaxAmmo > 0 && GameObject.Find("axeArms") == null)
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
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
                AudioSource.PlayClipAtPoint(impact, new Vector3(5, 1, 2));



                currentAmmo--;

                ammoText.text = currentAmmo + " / " + MaxAmmo;

                RaycastHit hit;

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    Targets targets = hit.transform.GetComponent<Targets>();

                    
                    if (targets != null)
                    {
                       // targets.transform.Rotate(1000,1000,1000, Space.Self);
                        run.speed = 4;
                        targets.TakeDamage(damage);
                        Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 0.5f);
                    }
                    else
                    {
                        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        obj.transform.position += obj.transform.forward / 1000;
                        Destroy(obj, 4f);
                    }
                }

            }
        }
        else if (!isAttacking && !isRunning && player.transform.position.x == startPosX && !isReloading)
        {
            ChangeAnimationState(IDLE);
            secundomer = 0;
        }
        else if (isRunning && !isAttacking && secundomer > 0.15 && !isReloading)
        {
            ChangeAnimationState(enableRUN);
        }
        else if (!isRunning && !isAttacking && player.transform.position.x != startPosX && secundomer > 0.2 && !isReloading)
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
        run.speed = 6;
    }
}