using System.Collections;
using UnityEngine;
public class AxeScript : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int _damage = 65;
    [SerializeField] private float _attackDelay = 1.8f;

    [Header("GameObjects")]
    [SerializeField] private GameObject _blood;
    private Collider _axeCollider;

    [Header("Delay")]
    [SerializeField] private float _delayEndbleCollider = 0.2f;
    [SerializeField] private float _impactDuration = 0.33f;

    [Header("Sound")]
    [SerializeField] private AudioClip[] _missSounds;
    [SerializeField] private AudioClip[] _enemyHitSounds;
    [SerializeField] private AudioClip _wordHitSound;
    [SerializeField] private float _enemyHitSoundVolume = 0.1f;
    [SerializeField] private float _missSoundVolume = 0.2f;
    [SerializeField] private float _wordHitSoundVolume = 0.2f;

    private AudioSource _audioSource;
    private int _enemyHitSoundIndex = 0;
    private int _missSoundIndex = 0;

    // private GameObject _axe;

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

        //   _axe = GameObject.FindGameObjectWithTag("axeArms");
        _axeCollider = GetComponent<Collider>();

        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        _idleAnimation = clips[0].name;
        _fireAnimation = clips[3].name;

        _attackDelay = clips[3].length;
    }
    private void OnEnable() => ChangeAnimationState(_idleAnimation);
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && _animator.GetCurrentAnimatorStateInfo(0).IsName(_idleAnimation)) // && _axe.activeSelf
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
            _audioSource.PlayOneShot(_missSounds[ChangeMissSound()], _missSoundVolume);
            _isSoundEnabled = false;
        }
        Actions.AxeShoot(true);
        ChangeAnimationState(_fireAnimation);
        yield return new WaitForSeconds(_delayEndbleCollider);

        _axeCollider.enabled = true;

        yield return new WaitForSeconds(_impactDuration);

        _axeCollider.enabled = false;
        yield return new WaitForSeconds(_attackDelay - _impactDuration - _delayEndbleCollider);
        
        AttackComplete();
        yield return new WaitForSeconds(2f);

        Actions.AxeShoot(false);
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
            _audioSource.PlayOneShot(_enemyHitSounds[ChangeHitSound()], _enemyHitSoundVolume);
            _getHit = true;
            StartCoroutine(GetHitDefault());
        }
        else if (other.CompareTag("floor") && !_getHit)
        {
            _audioSource.PlayOneShot(_wordHitSound, _wordHitSoundVolume);
            _getHit = true;
            StartCoroutine(GetHitDefault());
        }
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
    private int ChangeMissSound()
    {
        _missSoundIndex += 1;
        if (_missSoundIndex == _missSounds.Length)
        {
            _missSoundIndex = 0;
            return Random.Range(0, _missSounds.Length);
        }
        return _missSoundIndex;
    }
    private IEnumerator GetHitDefault()
    {
        yield return new WaitForSeconds(1f);
        _getHit = false;
    }
}