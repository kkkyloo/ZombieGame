using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class AiZombie : MonoBehaviour, IDamageable
{
    [Header("AI Settings")]
    [SerializeField] private float _hp = 99;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _hearShootRaduis = 15f;
    [SerializeField] private float _hearStepRaduis = 7f;
    [SerializeField] private float _radiusView = 4f;
    [SerializeField] private float _angleView = 110f;
    [SerializeField] private float _firstAttackDelay = 0.5f;
    [SerializeField] private GameObject _arm;

    private float _distanceToPlayer;
    private bool _canSeePlayer = false;
    private bool _isFirstAttackDelay = false;

    private NavMeshAgent _agent;
    private Behaviour _script;
    private Collider _collider;

    [Header("Animation Settings")]
    [SerializeField] private float _attackRangePlus = 0.5f;
    [SerializeField] private float _runSpeed = 5.5f;
    [SerializeField] private float _changeAnimDelay = 0.15f;
    private Animator _animator;

    [Header("Audio")]
    private AudioSource _audioSource;

    public bool GetDamage { get; set; } = false;

    private void OnEnable()
    {
        Actions.GunShoot += HearGunShoot;
        Actions.OnMoveSound2 += HearFootSteps;

        _playerTransform = Camera.main.transform;
    }
    private void OnDisable()
    {
        Actions.GunShoot -= HearGunShoot;
        Actions.OnMoveSound2 -= HearFootSteps;
    }
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _script = GetComponent<AiZombie>();
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponent<Collider>();
        _audioSource.enabled = false;
    }
    private void LateUpdate()
    {
        if (!_canSeePlayer)
        {
            _agent.speed = 0;
            FieldOfViewCheck();
        }
        else if (!_isFirstAttackDelay) StartCoroutine(FirstAttackDelay());
        else
        {
            StartCoroutine(SmoothSetDestonation());
            AttackPlayer();
        }

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void FieldOfViewCheck()
    {
        Vector3 playerTarget = (_playerTransform.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, playerTarget) < _angleView / 2)
        {
            float distance = Vector3.Distance(transform.position, _playerTransform.position);

            if (distance <= _radiusView)
            {
                if (!Physics.Raycast(transform.position, playerTarget, distance, _obstacleMask)) _canSeePlayer = true;

                else
                    _canSeePlayer = false;
            }
            else
                _canSeePlayer = false;
        }
        else
            _canSeePlayer = false;
    }

    private void UpdateRotationToPlayer()
    {
        Vector3 playerDirection = (_playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
    private void AttackPlayer()
    {
        _audioSource.enabled = true;
        _distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        _agent.speed = _runSpeed;

        if (_distanceToPlayer < _agent.stoppingDistance + _attackRangePlus)
        {
            UpdateRotationToPlayer();
            _audioSource.enabled = false;
            _agent.speed = 0f;
            StartCoroutine(ChangeAnimation(true));
        }
        else StartCoroutine(ChangeAnimation(false));
    }
    private void HearGunShoot()
    {
        float hearDistance = Vector3.Distance(transform.position, _playerTransform.position);
        if (hearDistance < _hearShootRaduis) StartCoroutine(FirstAttackDelay());
    }
    private void HearFootSteps()
    {
        float hearDistance = Vector3.Distance(transform.position, _playerTransform.position);
        if (hearDistance < _hearStepRaduis) StartCoroutine(FirstAttackDelay());
    }
    private IEnumerator FirstAttackDelay()
    {
        yield return new WaitForSeconds(_firstAttackDelay);
        _canSeePlayer = true;
        _isFirstAttackDelay = true;
    }
    private IEnumerator SmoothSetDestonation()
    {
        if (_hp <= 0) yield break;
        yield return new WaitForSeconds(1f);
        if (_hp <= 0) yield break;
        _agent.SetDestination(_playerTransform.position);
    }


    private IEnumerator ChangeAnimation(bool state)
    {
        yield return new WaitForSeconds(_changeAnimDelay);
        _animator.SetBool("Attack", state);
    }
    public void TakeDamage(float damage)
    {
        if (_hp - damage > 0)
        {
            _hp -= damage;
            _canSeePlayer = true;
            _isFirstAttackDelay = true;
        }
        else Die();
    }
    private void Die()
    {
        _hp = 0;
        _agent.enabled = false;
        _animator.enabled = false;
        _script.enabled = false;
        _collider.enabled = false;
        _audioSource.enabled = false;
        _agent.speed = 0f;
        _arm.GetComponent<Collider>().enabled = false;
        _arm.GetComponent<ArmDamage>().enabled = false;

        Destroy(gameObject, 7f);
    }
}