using System.Collections;
using UnityEngine;

public class Ak47Script : MonoBehaviour, IWeapon
{
    [Header("Gun Settings")]
    [SerializeField] private float _damage = 15;
    [SerializeField] private float _attackDelay = 0.11f;
    [SerializeField] private float _attackRange = 20; 
    [SerializeField] private int _reserveAmmo = 60; // общий запас патронов
    [SerializeField] private int _currentAmmo = 30; // патронов в магазине

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

    [SerializeField] private AudioClip[] _enemyHitSounds;
    [SerializeField] private float _enemyHitSoundVolume = 0.1f;
    private int _enemyHitSoundIndex = 0;

    [Header("Muzzle Flash")]
    [SerializeField] private ParticleSystem _particleFlash;
    [SerializeField] private float _bulletHoleDestrTime = 4f;

    [Header("GameObjects")]
    [SerializeField] private GameObject _bulletHolePrefab;
    [SerializeField] private GameObject _bulletHolePrefab2;
    [SerializeField] private GameObject _prefab;
    private GameObject _camera;

    [Header("Animation")]
    public static Animator _animator;
    private string _currentAnimation;
    public static string IDLE;

    private bool running = false;

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
        akSound = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _camera = GameObject.FindWithTag("MainCamera");

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        IDLE = clips[0].name;
    }

    public static bool scope = false;

    private void Update()
    {
        if (_isReloading) return;

        // Если магазин пуст, но запас (_reserveAmmo) > 0, автоперезарядка
        if (_currentAmmo == 0 && _reserveAmmo > 0)
        {
            _animator.SetBool("canScope", false);
            _animator.SetBool("scope", false);

            SwitchGun.CanSwitch = false;

            StartCoroutine(ReloadGun());
            _isReloading = true;
            return;
        }

        // Принудительная перезарядка
        if (Input.GetKey(KeyCode.R))
        {
            ForceReload();
        }

        // Прицеливание (ПКМ)
        if (Input.GetMouseButton(1))
        {
            scope = true;
            _animator.SetBool("scope", true);
        }
        else
        {
            scope = false;
            _animator.SetBool("scope", false);
        }

        // Бег
        if (MovePlayer.IsRunning && !_isAttackPressed)
        {
            _animator.SetBool("canFire", false);
            _animator.SetBool("canRun", true);
            running = true;
        }
        else
        {
            _animator.SetBool("canRun", false);
            running = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isAttackPressed) HandleAttack();
        else if (!_isAttacking && !_isReloading && !running) 
            ChangeAnimationState(IDLE);
    }

    public void Attack()
    {
        SwitchGun.CanSwitch = false;

        if (_currentAmmo > 0)
        {
            _isAttackPressed = true;
            _animator.SetBool("canFire", true);
        }
        else if (Input.GetButtonDown("Fire1") && _reserveAmmo == 0 && _currentAmmo == 0)
        {
            akSound.PlayOneShot(_noAmmoSound, _noAmmoSoundVolume);
        }
    }

    private IEnumerator ReloadGun()
    {
        akSound.PlayOneShot(_reloadSound, _reloadSoundVolume);
        _animator.SetTrigger("reload");

        yield return new WaitForSeconds(2);

        // Подсчёт, сколько нужно положить в магазин
        int amountToWithdraw = Mathf.Min(30 - _currentAmmo, _reserveAmmo);
        _reserveAmmo -= amountToWithdraw;
        _currentAmmo += amountToWithdraw;

        // Обновляем HUD 
        SwitchGun.Instance.SetAmmoText(_currentAmmo, _reserveAmmo);

        _isReloading = false;
        _animator.SetBool("canScope", true);

        ChangeAnimationState(IDLE);
        SwitchGun.CanSwitch = true;
    }

    private void ReloadDown()
    {
        if (_reserveAmmo > 0)
        {
            StartCoroutine(ReloadGun());
            _isReloading = true;
        }
    }

    public void ForceReload()
    {
        // Если ещё не идёт перезарядка и есть патроны в резерве, а магазин не полон
        if (!_isReloading && _reserveAmmo > 0 && _currentAmmo < 30)
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
        _animator.SetBool("canFire", false);

        if (!_isAttacking)
        {
            _isAttacking = true;
            Invoke(nameof(AttackComplete), _attackDelay);

            akSound.PlayOneShot(_shootSound, _shootSoundVolume);
            akSound.pitch = Random.Range(1f, 1.1f);

            // Минус один патрон
            _currentAmmo--;

            // Обновляем HUD
            SwitchGun.Instance.SetAmmoText(_currentAmmo, _reserveAmmo);

            // Луч стрельбы
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _attackRange))
            {
                HandleHitResult(hit);
            }
        }

        SwitchGun.CanSwitch = true;
    }

    private void PlayAttackAnimation()
    {
        _particleFlash.Play();
        _animator.SetTrigger("fire");
    }

    private void HandleHitResult(RaycastHit hit)
    {
        // Логика попадания: зомби, объекты и т.д.

        if (hit.transform.TryGetComponent<AiZombie>(out var targets) && targets.enabled)
        {
            targets.TakeDamage(GetDamageByRange(hit.distance));

            // Проигрываем звук попадания
            AudioSource.PlayClipAtPoint(_enemyHitSounds[ChangeHitSound()], 
                                        hit.transform.position, 
                                        _enemyHitSoundVolume);

            Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 0.5f);
        }
        else if (!hit.transform.CompareTag("Enemy"))
        {
            CreateBulletHole(hit);
        }
    }

    private int ChangeHitSound()
    {
        _enemyHitSoundIndex++;
        if (_enemyHitSoundIndex == _enemyHitSounds.Length)
        {
            _enemyHitSoundIndex = 0;
            return Random.Range(0, _enemyHitSounds.Length);
        }
        return _enemyHitSoundIndex;
    }

    private void CreateBulletHole(RaycastHit hit)
    {
        GameObject obj = Instantiate(_bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        obj.transform.position += obj.transform.forward / 1000;
        Destroy(obj, _bulletHoleDestrTime);

        GameObject obj2 = Instantiate(_bulletHolePrefab2, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(obj2, _bulletHoleDestrTime);
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;
        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }

    private void AttackComplete()
    {
        _isAttacking = false;
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
