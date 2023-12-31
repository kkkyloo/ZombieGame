using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;

    private bool _readyToJump;

    [SerializeField] private Joystick _joystick;


    [Header("_ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _ground;
    private bool _grounded;

    [SerializeField] private Transform _orientation;

    public static float horizontalInput;
    public static float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private TextMeshProUGUI text_speed;

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public static bool jumpPress = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        _readyToJump = true;
    }

    private void FixedUpdate()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        MyInput();
        SpeedControl();

        if (_grounded)
            rb.drag = _groundDrag;
        else
            rb.drag = 0;

        MovePlayer();
    }

  

    public void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //horizontalInput = _joystick.Horizontal;
        //verticalInput = _joystick.Vertical;

        if (jumpPress && _readyToJump && _grounded)
        {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (_grounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        }

        else if (!_grounded)
        {
            rb.AddForce(moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    public void OnJumpButtonDown() => jumpPress = true;
    public void OnJumpButtonUp() => jumpPress = false;
    private void ResetJump() => _readyToJump = true;
    private Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal.normalized);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }
}