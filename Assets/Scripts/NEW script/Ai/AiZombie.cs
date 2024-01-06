using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class AiZombie : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform[] _wayPoins;
    [SerializeField] private float _attackRangePlus = 0.5f;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _runSpeed = 7f;
    [SerializeField] private float _distanceSee = 10f;
    [SerializeField] private float _hp = 100;
    [SerializeField] private int _distanceToWayPoint = 2;

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
            _animator.SetBool("Attack", true);
        }
        else _animator.SetBool("Attack", false);
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
}