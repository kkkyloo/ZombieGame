using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class GrenadeObject : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float _maxDamage = 100;
    [SerializeField] private float _explosionRadius = 10;
    [SerializeField] private float _explosionDelay = 3;
    [SerializeField] private float _explosionForce = 500;

    [Header("Game Objects")]
    [SerializeField] private GameObject _explosionPrefab;

    private Rigidbody rb;

    private bool _hasExploded = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            rb.linearVelocity *= 0.98f;
        }
    }

    private void Update()
    {
        _explosionDelay -= Time.deltaTime;
        if (_explosionDelay <= 0f && !_hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] hitColliders = new Collider[10];

        int collidersHit = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, hitColliders);
        for (int i = 0; i < collidersHit; i++)
        {
            if (hitColliders[i].TryGetComponent(out Rigidbody rb))
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
            if (hitColliders[i].TryGetComponent(out IDamageable targets))
            {
                targets.TakeDamage(_maxDamage - Vector3.Distance(transform.position, hitColliders[i].transform.position) * (_maxDamage / _explosionRadius));
            }
        }

        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
 
