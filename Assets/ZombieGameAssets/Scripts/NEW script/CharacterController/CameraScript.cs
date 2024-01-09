using System.Collections;
using UnityEngine;
public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform _orientation;
    [Header("Camera settings")]
    [SerializeField] private float _sensitivity = 2f;
    [SerializeField] private float _smoothing = 2f;
    [SerializeField] private float _angleTilt = 2f;
    [SerializeField] private float _angleImpact = 1f;
    [SerializeField] private float _speedTilt = 5f;
    [SerializeField] private int[] _angleDamage = { -250, 250 };
    [SerializeField] private float _angelAxeTime;

    private float _xRotation = 0;
    private float _yRotation = 0;
    private float _horizontalInput = 0;
    private float currentAngleTilt = 0;

    private Vector2 _smoothV;
    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Actions.OnMove += AngleChange;
        Actions.GunShoot += Impact;
        Actions.GetEnemyHit += GetDamage;
        Actions.AxeShoot += AxeImpact;
    }
    private void OnDisable()
    {
        Actions.OnMove -= AngleChange;
        Actions.GunShoot -= Impact;
        Actions.GetEnemyHit -= GetDamage;
        Actions.AxeShoot -= AxeImpact;
    }
    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(_sensitivity * _smoothing, _sensitivity * _smoothing));
        _smoothV.x = Mathf.Lerp(_smoothV.x, mouseDelta.x, 1f / _smoothing);
        _smoothV.y = Mathf.Lerp(_smoothV.y, mouseDelta.y, 1f / _smoothing);

        _yRotation += _smoothV.x;
        _xRotation -= _smoothV.y;

        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, currentAngleTilt);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
    private void AngleChange(float horizontalInput, float v)
    {
        _horizontalInput = horizontalInput;

        if (_horizontalInput < 0) currentAngleTilt = Mathf.Lerp(currentAngleTilt, _angleTilt, Time.deltaTime * _speedTilt);
        else if (_horizontalInput > 0) currentAngleTilt = Mathf.Lerp(currentAngleTilt, -_angleTilt, Time.deltaTime * _speedTilt);
        else currentAngleTilt = Mathf.Lerp(currentAngleTilt, 0.0f, Time.deltaTime * _speedTilt);
    }
    private void Impact() => StartCoroutine(DoImpact());
    IEnumerator DoImpact()
    {
        yield return null;
        transform.rotation = Quaternion.Euler(
            new Vector3(
            _xRotation + Random.Range(-_angleImpact, _angleImpact),
            _yRotation + Random.Range(-_angleImpact, _angleImpact),
            currentAngleTilt + Random.Range(-_angleImpact, _angleImpact)));
    }
    private void GetDamage(int damage)
    {
        if (damage < 20)
            currentAngleTilt = Mathf.Lerp(currentAngleTilt, _angleDamage[Random.Range(0, 2)], Time.deltaTime * 7);
        else if (damage > 20)
            currentAngleTilt = Mathf.Lerp(currentAngleTilt, _angleDamage[Random.Range(0, 2)] + 50, Time.deltaTime * 7);
    }
    private void AxeImpact(bool z) // передавать состояние атаки. если не атакует, то в элс вернуть к изначальному
    {
        if(z)
            currentAngleTilt = Mathf.Lerp(currentAngleTilt, _angleTilt, Time.deltaTime * _speedTilt);  //нужно из изначального в мое, а не из моего в изначальное
        else currentAngleTilt = Mathf.Lerp(currentAngleTilt, 0.0f, Time.deltaTime * _speedTilt);
    }
}