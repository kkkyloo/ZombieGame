using System.Collections;
using UnityEngine;
public class Ak47Script : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float _damage = 15;
    [SerializeField] private float _attackDelay = 0.11f;
    [SerializeField] private float _attackRange = 20; // чем дальше, тем меньше урона (нужно хедшоты реализовать)
    [SerializeField] private int _reserveAmmo = 60;
    [SerializeField] private int _currentAmmo = 30;

    [Header("Audio")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _noAmmoSound;
    [SerializeField] private AudioClip _changeGunSound;
    [SerializeField] private float _shootSoundVolume = 1f;
    [SerializeField] private float _reloadSoundVolume = 1f;
    [SerializeField] private float _noAmmoSoundVolume = 1f;
    [SerializeField] private float _changeGunSoundVolume = 1f;
    private AudioSource akSound;

    [Header("Muzzle Flash")]
    [SerializeField] private ParticleSystem _particleFlash;
    [SerializeField] private float _bulletHoleDestrTime = 4f;

    [Header("GameObjects")]
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private GameObject _prefab;
    private GameObject _camera;

    [Header("Animation")]
    public static Animator _animator;
    private string _currentAnimation;
    public static string IDLE;
    private string FIRE;
    private string reloadAnim;

    private bool _isAttackPressed;
    private bool _isAttacking;
    private bool _isReloading = false;

    private void OnEnable()
    {
        ChangeAnimationState(IDLE);
        akSound.PlayOneShot(_changeGunSound, _changeGunSoundVolume);
    }
    private void Awake()
    {
        akSound = gameObject.AddComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _camera = GameObject.FindWithTag("MainCamera");

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        IDLE = clips[0].name.ToString();
        FIRE = clips[1].name.ToString();
        reloadAnim = clips[5].name.ToString();
    }
    private void Update()
    {
        if (_isReloading) return;

        if (_currentAmmo == 0 && _reserveAmmo > 0)
        {
            StartCoroutine(ReloadGun());
            _isReloading = true;
            return;
        }

        if (Input.GetButton("Fire1") && _currentAmmo > 0) _isAttackPressed = true;
        else if (Input.GetButtonDown("Fire1") && _reserveAmmo == 0 && _currentAmmo == 0) akSound.PlayOneShot(_noAmmoSound, _noAmmoSoundVolume);

    }
    private void FixedUpdate()
    {
        if (_isAttackPressed) HandleAttack();
        else if (!_isAttacking && !_isReloading) ChangeAnimationState(IDLE);
    }
    private IEnumerator ReloadGun()
    {
        akSound.PlayOneShot(_reloadSound, _reloadSoundVolume);
        ChangeAnimationState(reloadAnim);

        yield return new WaitForSeconds(2);

        int amountToWithdraw = Mathf.Min(30 - _currentAmmo, _reserveAmmo);
        _reserveAmmo -= amountToWithdraw;
        _currentAmmo += amountToWithdraw;

        _isReloading = false;
        ChangeAnimationState(IDLE);
    }
    private void ReloadDown()
    {
        if (_reserveAmmo > 0) //  && GameObject.Find("axeArms") == null
        {
            StartCoroutine(ReloadGun());
            _isReloading = true;
        }
    }
    private void HandleAttack()
    {
        Actions.GunShoot();

        PlayAttackAnimation();
        _isAttackPressed = false;

        if (!_isAttacking)
        {
            _isAttacking = true;
            Invoke(nameof(AttackComplete), _attackDelay);
            akSound.PlayOneShot(_shootSound, _shootSoundVolume);
            akSound.pitch = Random.Range(1f, 1.1f);

            //AudioSource.PlayClipAtPoint(_shootSound, new Vector3(2, 2, 2));

            _currentAmmo--;

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _attackRange))
            {
                HandleHitResult(hit);
            }
        }
    }
    private void PlayAttackAnimation()
    {
        _particleFlash.Play();
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
        else if (!hit.transform.CompareTag("Enemy")) CreateBulletHole(hit);
    }
    private void CreateBulletHole(RaycastHit hit)
    {
        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        obj.transform.position += obj.transform.forward / 1000;
        Destroy(obj, _bulletHoleDestrTime);
    }
    private void ChangeAnimationState(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
    private void AttackComplete() // в зависимоти от eange урон
    {
        _isAttacking = false;
        //      run.speed = 6;
    }
    private float GetDamageByRange(float distance)
    {
        if (distance <= 5)
            return _damage;
        else if (distance <= 20)
            return _damage - 5;
        else
            return _damage - 10;
    }
}