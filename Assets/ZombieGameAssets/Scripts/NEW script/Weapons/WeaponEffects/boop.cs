using UnityEngine;
public class Boop : MonoBehaviour
{
    [Range(0.001f, 0.01f)]
    [SerializeField] private float _amount = 0.00596f;
    [Range(1f, 30f)]
    [SerializeField] private float _freq = 20.5f;
    [Range(10f, 100f)]
    [SerializeField] private float _smooth = 19f;

    private Vector3 _startPos;
    private Vector2 _inputMagnitude = Vector2.zero;


    private void OnEnable()
    {
        Actions.GunShoot += StartHeadBob;
        Actions.OnMove += CheckForHeadbobTrigger;
    }
    private void OnDisable()
    {
        Actions.GunShoot -= StartHeadBob;
        Actions.OnMove -= CheckForHeadbobTrigger;
    }

    private void Awake() => _startPos = transform.localPosition;
    void Update()
    {
        if (Ak47Script.scope && MovePlayer.rolling)
        {

            StopHeadBob();
            return;
        }



        if (Vector3.Distance(transform.localPosition, _startPos) > 0.01f || Ak47Script.scope && !MovePlayer.moving) StopHeadBob();
        if (_inputMagnitude.magnitude > 0) StartHeadBob();
    }
    private void StopHeadBob() => transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, _smooth * Time.deltaTime);
    private void CheckForHeadbobTrigger(float horizontalInput, float verticalInput) => _inputMagnitude = new Vector2(Mathf.Abs(horizontalInput), Mathf.Abs(verticalInput));
    private void StartHeadBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * _freq) * _amount * 1.4f;
        pos.x = Mathf.Cos(Time.time * _freq / 2) * _amount * 1.6f;

        transform.localPosition += _smooth * Time.deltaTime * pos;
    }
}