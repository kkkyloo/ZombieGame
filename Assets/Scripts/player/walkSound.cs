using UnityEngine;
public class WalkSound : MonoBehaviour
{
    private AudioSource _sound;
    private void OnEnable() => Actions.OnMoveSound += PlaySound;
    private void OnDisable() => Actions.OnMoveSound -= PlaySound;
    private void Awake() => _sound = GetComponent<AudioSource>();
    private void PlaySound(float horizontalInput, float verticalInput, bool _isJumpPress)
    {
        if (Mathf.Abs(horizontalInput) > 0  && !_isJumpPress || Mathf.Abs(verticalInput) > 0 && !_isJumpPress)
        {
            _sound.pitch = Random.Range(0.8f, 1.1f);
            _sound.enabled = true;
        }
        else _sound.enabled = false;
    }
} 