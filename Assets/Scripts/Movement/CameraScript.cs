using UnityEngine;
public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private float sensitivity = 2;
    [SerializeField] private float smoothing = 2;
    [SerializeField] private float _angleTilt;

    private float xRotation = 0;
    private float yRotation = 0;

    private Vector2 smoothV;

    private float _horizontalInput = 0;

    private float currentAngleTilt = 0;
    [SerializeField] private float tiltSpeed = 5;

    private void OnEnable()
    {
        Actions.OnMove += AngleChange;
    }
    private void OnDisable()
    {
        Actions.OnMove -= AngleChange;
    } 
    private void Update(){
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);

        yRotation += smoothV.x;

        xRotation -= smoothV.y; 
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, currentAngleTilt);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    private void AngleChange(float horizontalInput, float v)
    {
        _horizontalInput = horizontalInput;

        if (_horizontalInput < 0) currentAngleTilt = Mathf.Lerp(currentAngleTilt, _angleTilt, Time.deltaTime * tiltSpeed);        
        else if (_horizontalInput > 0) currentAngleTilt = Mathf.Lerp(currentAngleTilt, -_angleTilt, Time.deltaTime * tiltSpeed);
        else currentAngleTilt = Mathf.Lerp(currentAngleTilt, 0.0f, Time.deltaTime * tiltSpeed);
    }
} 