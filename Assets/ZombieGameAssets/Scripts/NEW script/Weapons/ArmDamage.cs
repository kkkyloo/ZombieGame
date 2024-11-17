using System.Collections;
using UnityEngine;
public class ArmDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float _delayDamage = 0.7f; // переключение в idle
    [SerializeField] private float _delayPreDamage = 0.3f;
    [SerializeField] private int _damage = 10; // урон если переключено в атаку и прошло время

    [Header("Sound hit")]
    [SerializeField] private AudioClip[] _enemyHitSounds;
    [SerializeField] private float _enemyHitSoundVolume = 0.1f;
    private AudioSource _audioSource;

    private bool _doHit = false;
    private bool _exitPlayerCollider = false;
    private bool _damageDelayRunning = false;

    private void Awake() => _audioSource = gameObject.AddComponent<AudioSource>();
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
            _audioSource.PlayOneShot(_enemyHitSounds[ChangeHitSound()], _enemyHitSoundVolume);

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
    private int ChangeHitSound() => Random.Range(0, _enemyHitSounds.Length - 1);
}