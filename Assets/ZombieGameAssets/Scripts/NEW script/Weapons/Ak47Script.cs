using System.Collections;
using UnityEngine;

public class Ak47Script : MonoBehaviour, IWeapon
{
    [Header("Gun Settings")]
    [SerializeField] private float _damage = 15;
    [SerializeField] private float _attackDelay = 0.11f;
    [SerializeField] private float _attackRange = 20; // ��� ������, ��� ������ ����� (����� ������� �����������)
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
    [SerializeField] private AudioClip[] _enemyHitSounds;
    [SerializeField] private float _enemyHitSoundVolume = 0.1f;
    private int _enemyHitSoundIndex = 0;

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

    private bool running = false;

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
        akSound = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _camera = GameObject.FindWithTag("MainCamera");

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        IDLE = clips[0].name.ToString();
        reloadAnim = clips[5].name.ToString();
    }

    public static bool scope = false;

    private void Update()
    {
        if (_isReloading) return;

        if (_currentAmmo == 0 && _reserveAmmo > 0)
        {
            _animator.SetBool("canScope", false);

            SwitchGun.CanSwitch = false;

            StartCoroutine(ReloadGun());
            _isReloading = true;
            return;
        }

        if (Input.GetKey(KeyCode.R))
        {
            ForceReload();
        }

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



        if (MovePlayer.IsRunning && !_isAttackPressed)
        {
            _animator.SetBool("canFire", false);

            _animator.SetBool("canRun", true);
            running = true;
        }
        else
        {
            _animator.SetBool("canRun", false);
        }

    }
    private void FixedUpdate()
    {
        if (_isAttackPressed) HandleAttack();
        else if (!_isAttacking && !_isReloading && !running) ChangeAnimationState(IDLE);
    }


    public void Attack()
    {
        SwitchGun.CanSwitch = false;

        if (_currentAmmo > 0)
        {
            _isAttackPressed = true;
            _animator.SetBool("canFire", true);


        }
        else if (Input.GetButtonDown("Fire1") && _reserveAmmo == 0 && _currentAmmo == 0) akSound.PlayOneShot(_noAmmoSound, _noAmmoSoundVolume);


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
        _animator.SetBool("canScope", true);

        ChangeAnimationState(IDLE);
        SwitchGun.CanSwitch = true;

    }
    private void ReloadDown()
    {
        if (_reserveAmmo > 0) //  && GameObject.Find("axeArms") == null
        {
            StartCoroutine(ReloadGun());
            _isReloading = true;
        }
    }

    public void ForceReload()
    {
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

            //AudioSource.PlayClipAtPoint(_shootSound, new Vector3(2, 2, 2));

            _currentAmmo--;

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

        //ChangeAnimationState(FIRE);
    }
    private void HandleHitResult(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent<AiZombie>(out var targets) && targets.enabled)
        {
            targets.TakeDamage(GetDamageByRange(hit.distance));

            // akSound.PlayOneShot(_enemyHitSounds[ChangeHitSound()], _enemyHitSoundVolume);
            AudioSource.PlayClipAtPoint(_enemyHitSounds[ChangeHitSound()], hit.transform.position, _enemyHitSoundVolume);

            // Actions.OnHitEnemy(GetDamageByRange(hit.distance), hit.collider.gameObject);
            Destroy(Instantiate(_prefab, hit.point, Quaternion.identity), 0.5f);
        }
        else if (!hit.transform.CompareTag("Enemy")) CreateBulletHole(hit);
    }
    private int ChangeHitSound()
    {
        _enemyHitSoundIndex += 1;
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
    }
    private void ChangeAnimationState(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
    private void AttackComplete() // � ���������� �� eange ����
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