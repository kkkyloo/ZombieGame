using System.Collections;
using UnityEngine;
public class Shootgun : MonoBehaviour, IWeapon
{
    [Header("Gun Settings")]
    [SerializeField] private float damage = 51f;
    [SerializeField] private float attackDelay = 0.45f;
    [SerializeField] private float range = 40; // чем дальше, тем меньше урона (нужно хедшоты реализовать)
    [SerializeField] private int MaxAmmo = 12;
    [SerializeField] private int _currentAmmo = 4;

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
    public static Animator _animator;
    private string currentAnimation;
    public static string IDLE;
    private string FIRE;
    private string reloadAnim;

    private bool _isAttackPressed;
    private bool isAttacking;
    private bool isReloading = false;

    private float bulletHoleDestroyTime = 4f;

    private void OnEnable() => ChangeAnimationState(IDLE);
    private void Awake()
    {
        akSound = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        fpsCam = GameObject.FindWithTag("MainCamera");

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        IDLE = clips[0].name.ToString();
        FIRE = clips[3].name.ToString();
        reloadAnim = clips[4].name.ToString();
    }
    private void Update()
    {
        if (isReloading) return;

        if (_currentAmmo == 0 && MaxAmmo > 0)
        {
            StartCoroutine(ReloadGun());
            isReloading = true;
            return;
        }

    }

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && _currentAmmo > 0)
        {
            _isAttackPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isAttackPressed) HandleAttack();
        else if (!isAttacking && !isReloading) ChangeAnimationState(IDLE);
    }
    private IEnumerator ReloadGun()
    {
        akSound.PlayOneShot(reload, reloadSoundVolume);
        ChangeAnimationState(reloadAnim);

        yield return new WaitForSeconds(2);

        int amountToWithdraw = Mathf.Min(30 - _currentAmmo, MaxAmmo);
        MaxAmmo -= amountToWithdraw;
        _currentAmmo += amountToWithdraw;

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
        SwitchGun.CanSwitch = false;

        Actions.GunShoot();

        PlayAttackAnimation();
        _isAttackPressed = false;

        if (!isAttacking)
        {
            isAttacking = true;
            Invoke(nameof(AttackComplete), attackDelay);
            akSound.PlayOneShot(impact, reloadSoundVolume);

            //AudioSource.PlayClipAtPoint(impact, new Vector3(5, 1, 2));

            _currentAmmo--;

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, range))
            {
                HandleHitResult(hit);
            }

            SwitchGun.CanSwitch = true;

        }
    }
    private void PlayAttackAnimation()
    {
        flash.Play();
        ChangeAnimationState(FIRE);
    }
    private void HandleHitResult(RaycastHit hit)
    {
    //    if (hit.transform.TryGetComponent<Targets>(out var targets))
    //    {
    //        run.speed = 4;
            //targets.TakeDamage(damage);
   //         targets.TakeDamage(GetDamageByRange(hit.distance));

   //         Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 0.5f);
    //    }
    //    else CreateBulletHole(hit);
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

        _animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
    private void AttackComplete() // в зависимоти от eange урон
    {
        isAttacking = false;
    //    run.speed = 6;
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