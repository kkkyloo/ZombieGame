using System.Collections;
using UnityEngine;
public class WalkSound : MonoBehaviour
{
    private AudioSource _sound;
    [Range(0.01f, 1.9f)]
    [SerializeField] private float _startRange = 0.7f;
    [Range(0.01f, 1.9f)]
    [SerializeField] private float _endRange = 1f;
    [SerializeField] private float _delay = 0.15f;

    private void OnEnable() => Actions.OnMoveSound += PlaySound;
    private void OnDisable() => Actions.OnMoveSound -= PlaySound;
    private void Awake() => _sound = GetComponent<AudioSource>();
    private void PlaySound(float horizontalInput, float verticalInput, bool _isJumpPress)
    {
        if (Mathf.Abs(horizontalInput) > 0 && !_isJumpPress || Mathf.Abs(verticalInput) > 0 && !_isJumpPress)
        {
            _sound.pitch = Random.Range(_startRange, _endRange);
            _sound.enabled = true;
        }
        else
        {
            if (_sound.enabled) StartCoroutine(Disabledelay());
        }
    }
    private IEnumerator Disabledelay()
    {
        yield return new WaitForSeconds(_delay);
        _sound.enabled = false;
    }
}