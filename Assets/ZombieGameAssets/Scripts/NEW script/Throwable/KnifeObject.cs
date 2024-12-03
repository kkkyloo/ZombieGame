using UnityEngine;

public class KnifeObject : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private float _damage = 30;
    private Rigidbody _rb;
    private bool _targetHit;

    private void Start()
    {
       _rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {



        if (_targetHit)
            return;
        else
            _targetHit = true;

        _rb.isKinematic = true;
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(_damage);
        }
        transform.SetParent(collision.transform);
    }

}
