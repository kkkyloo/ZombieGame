using TMPro;
using UnityEngine;
using System.Collections;

public class shotgun : MonoBehaviour
{
    [SerializeField] private float damage = 51f;
    [SerializeField] private float attackDelay = 0.1f;
    [SerializeField] private float range = 60;
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private AudioClip impact;

    [SerializeField] public static int MaxAmmo = 12;
    [SerializeField] public static int currentAmmo = 3;

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

    private bool readyToShoot = true;
    [SerializeField] private AudioClip reload;

    private void Start()
    {
        animator = GetComponent<Animator>();
        akSound = GetComponent<AudioSource>();
        fpsCam = GameObject.Find("PlayerCam").GetComponent<Camera>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        IDLE = clips[0].name.ToString();
        FIRE = clips[3].name.ToString();
        enableRUN = clips[1].name.ToString();
        disableRUN = clips[2].name.ToString();

        reloadAnim = clips[4].name.ToString();

        ammoText = GameObject.Find("ammoSG").GetComponent<TextMeshProUGUI>();
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


        if (currentAmmo == 0 && MaxAmmo > 0 && currentAmmo != 3)
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
            return;
        }

        if (PlayerCam.fire == true && currentAmmo > 0 && readyToShoot)
        {
            isAttackPressed = true;
            startPosX = player.transform.position.x;
            startPosY = player.transform.position.y;
            animComplite = false;
            readyToShoot = false;
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


        int amountToWithdraw = Mathf.Min(3 - currentAmmo, MaxAmmo);
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
                akSound.PlayOneShot(impact, 0.7F);



                currentAmmo--;

                ammoText.text = currentAmmo + " / " + MaxAmmo;

                RaycastHit hit;

                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 5))
                {
                    Targets targets = hit.transform.GetComponent<Targets>();


                    if (targets != null)
                    {
                        // targets.transform.Rotate(1000,1000,1000, Space.Self);
                        run.speed = 4;
                        targets.TakeDamage(70);
                        //Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 2.5f);
                    }
                    else
                    {
                        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        obj.transform.position += obj.transform.forward / 1000;
                        Destroy(obj, 4f);
                    }
                }

                else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 20))
                {
                    Targets targets = hit.transform.GetComponent<Targets>();


                    if (targets != null)
                    {
                        // targets.transform.Rotate(1000,1000,1000, Space.Self);
                        run.speed = 4;
                        targets.TakeDamage(30);
                        //Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 2.5f);
                    }
                    else
                    {
                        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
                        obj.transform.position += obj.transform.forward / 1000;
                        Destroy(obj, 4f);
                    }
                }
                else if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 60))
                {
                    Targets targets = hit.transform.GetComponent<Targets>();


                    if (targets != null)
                    {
                        // targets.transform.Rotate(1000,1000,1000, Space.Self);
                        run.speed = 4;
                        targets.TakeDamage(10);
                        //Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 2.5f);
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
        readyToShoot = true;
    }
}