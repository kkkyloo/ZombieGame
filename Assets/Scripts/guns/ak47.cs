using TMPro;
using UnityEngine;
using System.Collections;

public class Ak47 : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float range = 20;
    [SerializeField] public static int MaxAmmo = 60;
    [SerializeField] public static int currentAmmo = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip noAmmo;

    private AudioSource akSound;

    [Header("Muzzle Flash")]
    [SerializeField] private ParticleSystem flash;

    [Header("GameObjects")]
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private GameObject _prefab;

    [Header("UI")]
    [SerializeField] public static TextMeshProUGUI ammoText;

    [Header("Character and Camera")]
    [SerializeField] private Camera fpsCam;
    [SerializeField] private GameObject player;

    [Header("Animation")]
    public static Animator animator;
    private string currentAnimation;
    public static string IDLE;
    private string FIRE;
    private string enableRUN;
    private string disableRUN;
    private string reloadAnim;

    private bool isAttackPressed;
    private bool isAttacking;
    private bool isRunning;

    private float secundomer;
    public static bool animComplete = true;

    public static bool isReloading = false;

    private float startPosX;
    private float startPosY;

    private void OnEnable() => ChangeAnimationState(IDLE);

    private void Start()
    {
        akSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        IDLE = clips[0].name.ToString();
        FIRE = clips[1].name.ToString();
        enableRUN = clips[3].name.ToString();
        disableRUN = clips[4].name.ToString();

        reloadAnim = clips[5].name.ToString();
        ammoText = GameObject.Find("ammoAk").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isReloading) return;

        if (currentAmmo == 0 && MaxAmmo > 0)
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
            return;
        }

        if (PlayerCam.fire && currentAmmo > 0)
        {
            isAttackPressed = true;
            startPosX = player.transform.position.x;
            startPosY = player.transform.position.y;
            animComplete = false;
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

    private void FixedUpdate()
    {
        if (isAttackPressed)
        {
            HandleAttack();
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

    private void HandleAttack()
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

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    private void AttackComplete()
    {
        isAttacking = false;
        animComplete = true;
        run.speed = 6;
    }
}
