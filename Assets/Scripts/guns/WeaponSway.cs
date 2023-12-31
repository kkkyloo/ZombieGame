using UnityEngine;

public class WeaponSway : MonoBehaviour {

    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float multiplier;

    private void Update()
    {
        // get mouse input
     //   float mouseX = SimpleInput.GetAxis("Panel X") * multiplier;
    //    float mouseY = SimpleInput.GetAxis("Panel Y") * multiplier;

        // calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-Input.GetAxisRaw("Mouse Y") * multiplier, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * multiplier, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        // rotate 
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}