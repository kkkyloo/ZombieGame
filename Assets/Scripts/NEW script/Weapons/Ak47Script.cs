using System.Collections;
using UnityEngine;
public class Ak47Script : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float damage = 15;
    [SerializeField] private float attackDelay = 0.11f;
    [SerializeField] private float range = 20; // чем дальше, тем меньше урона (нужно хедшоты реализовать)
    [SerializeField] private int MaxAmmo = 60;
    [SerializeField] private int currentAmmo = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip impact;
    [SerializeField] private AudioClip reload;
    [SerializeField] private AudioClip noAmmo;
    [SerializeField] private float reloadSoundVolume = 1F;

    private AudioSource akSound;

    [Header("Muzzle Flash")]
    [SerializeField] private ParticleSystem flash;

    [Header("GameObjects")]
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private GameObject _prefab;

    private GameObject fpsCam;

    [Header("Animation")]
    public static Animator animator;
    private string currentAnimation;
    public static string IDLE;
    private string FIRE;
    private string reloadAnim;

    private bool isAttackPressed;
    private bool isAttacking;
    private bool isReloading = false;

    private float bulletHoleDestroyTime = 4f;

    private void OnEnable() => ChangeAnimationState(IDLE);
    private void Awake()
    {
        akSound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        fpsCam = GameObject.FindWithTag("MainCamera");

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        IDLE = clips[0].name.ToString();
        FIRE = clips[1].name.ToString();
        reloadAnim = clips[5].name.ToString();
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

        if (Input.GetButton("Fire1") && currentAmmo > 0) isAttackPressed = true;
    }
    private void FixedUpdate()
    {
        if (isAttackPressed) HandleAttack();
        else if (!isAttacking && !isReloading) ChangeAnimationState(IDLE);
    }
    private IEnumerator ReloadGun()
    {
        akSound.PlayOneShot(reload, reloadSoundVolume);
        ChangeAnimationState(reloadAnim);

        yield return new WaitForSeconds(2);

        int amountToWithdraw = Mathf.Min(30 - currentAmmo, MaxAmmo);
        MaxAmmo -= amountToWithdraw;
        currentAmmo += amountToWithdraw;

        isReloading = false;
        ChangeAnimationState(IDLE);
    }
    private void ReloadDown()
    {
        if (MaxAmmo > 0 && GameObject.Find("axeArms") == null)
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
        }
    }
    private void HandleAttack()
    {
        Actions.GunShoot();

        PlayAttackAnimation();
        isAttackPressed = false;

        if (!isAttacking)
        {
            isAttacking = true;
            Invoke(nameof(AttackComplete), attackDelay);
            akSound.PlayOneShot(impact, reloadSoundVolume);

            //AudioSource.PlayClipAtPoint(impact, new Vector3(5, 1, 2));

            currentAmmo--;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
            {
                HandleHitResult(hit);
            }
        }
    }
    private void PlayAttackAnimation()
    {
        flash.Play();
        ChangeAnimationState(FIRE);
    }
    private void HandleHitResult(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<AiZombie>(out var targets) && targets.enabled)
        {
            targets.TakeDamage(GetDamageByRange(hit.distance));
            // Actions.OnHitEnemy(GetDamageByRange(hit.distance), hit.collider.gameObject);
            Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 0.5f);
        }
        else if(!hit.transform.CompareTag("Enemy")) CreateBulletHole(hit);
    }
    private void CreateBulletHole(RaycastHit hit)
    {
        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        obj.transform.position += obj.transform.forward / 1000;
        Destroy(obj, bulletHoleDestroyTime);
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    private void AttackComplete() // в зависимоти от eange урон
    {
        isAttacking = false;
        //      run.speed = 6;
    }
    private float GetDamageByRange(float distance)
    {
        if (distance <= 5)
            return damage;
        else if (distance <= 20)
            return damage - 5;
        else
            return damage - 10;
    }
}