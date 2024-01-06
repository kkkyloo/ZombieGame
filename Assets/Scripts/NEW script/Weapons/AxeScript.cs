using System.Collections;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int _damage = 65;
    [SerializeField] private float _attackDelay = 1.8f;
    [Header("GameObjects")]
    [SerializeField] private GameObject _blood;
    [Header("Delay")]
    [SerializeField] private float _delayEndbleCollider = 0.2f;
    [SerializeField] private float _impactDuration = 0.33f;
    [Header("Sound")]
    [SerializeField] private AudioClip _airHit;
    [SerializeField] private AudioClip[] _enemyHitSound;
    [SerializeField] private float _enemyHitSoundVolume = 0.4f;
    [SerializeField] private float _airHitSound = 4f;

    private GameObject _axe;
    private Collider _axeCollider;
    private AudioSource _audioSource;
    private Animator _animator;

    private string _currentAnimation;
    private string _idleAnimation;
    private string _fireAnimation;

    private bool _isAttacking;
    private bool _isSoundEnabled = true;
    private bool _getHit = false;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _airHit;

        _axe = GameObject.FindGameObjectWithTag("axeArms");
        _axeCollider = GetComponent<Collider>();

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        _idleAnimation = clips[0].name;
        _fireAnimation = clips[3].name;

        _attackDelay = clips[3].length;
    }
    private void OnEnable() => ChangeAnimationState(_idleAnimation);
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _axe.activeSelf && _animator.GetCurrentAnimatorStateInfo(0).IsName(_idleAnimation))
            Attack();
    }
    private void Attack()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            StartCoroutine(PerformAttack());
        }
    }
    private IEnumerator PerformAttack()
    {
        if (_isSoundEnabled)
        {
            _audioSource.PlayOneShot(_airHit, _airHitSound);
            _isSoundEnabled = false;
        }

        ChangeAnimationState(_fireAnimation);
        yield return new WaitForSeconds(_delayEndbleCollider);

        _axeCollider.enabled = true;

        yield return new WaitForSeconds(_impactDuration);

        _axeCollider.enabled = false;
        yield return new WaitForSeconds(_attackDelay - _impactDuration - _delayEndbleCollider);

        AttackComplete();
    }
    private void AttackComplete()
    {
        _isSoundEnabled = true;
        _isAttacking = false;
        ChangeAnimationState(_idleAnimation);
    }
    private void ChangeAnimationState(string newAnimation)
    {
        if (_currentAnimation == newAnimation) return;

        _animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AiZombie>(out var targets) && !_getHit && targets.enabled)
        {
            targets.TakeDamage(_damage);
            Destroy(Instantiate(_blood, other.transform.position, Quaternion.identity), 0.5f);
            _audioSource.PlayOneShot(_enemyHitSound[Random.Range(0, _enemyHitSound.Length)], _enemyHitSoundVolume);
            _getHit = true;
            StartCoroutine(GetHitDefault());
        }
    }
    private IEnumerator GetHitDefault()
    {
        yield return new WaitForSeconds(1f);
        _getHit = false;
    }
}