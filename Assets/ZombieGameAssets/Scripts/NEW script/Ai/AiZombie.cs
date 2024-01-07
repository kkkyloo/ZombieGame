using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class AiZombie : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float _hp = 100;
    [SerializeField] private int _damage = 10; // урон если переключено в атаку и прошло время
    [SerializeField] private float _delayDamage = 2f; // переключение в idle
    [SerializeField] private float _delayPreDamage = 0.5f;
    private bool _exitPlayerCollider = false;
    private bool _damageDelayRunning = false;

    [SerializeField] private float _distanceSee = 14f;
    [SerializeField] private float _distanceToWayPoint = 2f;
    private bool _doHit = false;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform[] _wayPoins;
    [Header("Animation Settings")]
    [SerializeField] private float _attackRangePlus = 0.5f;
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _runSpeed = 5.5f;
    [SerializeField] private float _changeAnimDelay = 0.15f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Vector3 _target;

    private float _distanceToPlayer;
    private Behaviour _script;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _script = GetComponent<AiZombie>();

        WayPoint();
    }
    private void LateUpdate()
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (_distanceToPlayer > _distanceSee)
        {
            if (Vector3.Distance(transform.position, _target) < _distanceToWayPoint) StartCoroutine(SmoothChangeWayPoint());
            _agent.SetDestination(_target);
        }
        else AttackPlayer();

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
    private IEnumerator SmoothChangeWayPoint()
    {
        WayPoint();
        _agent.speed = 0f;
        yield return new WaitForSeconds(2f);

        _agent.speed = _walkSpeed;
    }
    private void WayPoint()
    {
        _agent.speed = _walkSpeed;
        _target = _wayPoins[Random.Range(0, _wayPoins.Length)].position;
        _agent.SetDestination(_target);
    }
    private void AttackPlayer()
    {
        _agent.SetDestination(_playerTransform.position);
        _agent.speed = _runSpeed;

        if (_distanceToPlayer < _agent.stoppingDistance + _attackRangePlus)
        {
            _agent.speed = 0f;
            StartCoroutine(ChangeAnimation(true));
        }
        else StartCoroutine(ChangeAnimation(false));
    }
    private IEnumerator ChangeAnimation(bool state)
    {
        yield return new WaitForSeconds(_changeAnimDelay);
        _animator.SetBool("Attack", state);
    }
    public void TakeDamage(float damage)
    {
        if (_hp - damage > 0) _hp -= damage;
        else Die();
    }
    private void Die()
    {
        _agent.enabled = false;
        _animator.enabled = false;
        _script.enabled = false;

        Destroy(gameObject, 7f);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !_doHit && !_damageDelayRunning)
        {
            _exitPlayerCollider = false;
            StartCoroutine(DamageDelay());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _exitPlayerCollider = true;
    }
    private IEnumerator DamageDelay()
    {
        _damageDelayRunning = true; 
        yield return new WaitForSeconds(_delayPreDamage);

        if (!_exitPlayerCollider)
        {
            Actions.GetEnemyHit(_damage);
            _doHit = true;
            StartCoroutine(GetHitDefault());
        }

        _damageDelayRunning = false;
    }
    private IEnumerator GetHitDefault()
    {
        yield return new WaitForSeconds(_delayDamage);
        _doHit = false;
    }
}