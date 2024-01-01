using UnityEngine;
public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Transform _orientation;

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;
    private Rigidbody _rigidBody;
    private RaycastHit _slopeHit;

    private bool _isExitingSlope;
    private bool _isReadyToJump;
    private bool _isGrounded;
    private bool _isJumpPress = false;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
        _isReadyToJump = true;
    }
    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        MyInput();
        SpeedControl();

        if (_isGrounded) _rigidBody.drag = _groundDrag;
        else _rigidBody.drag = 0;

        Move();
    }
    private void MyInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey("space"))
        {
            OnJumpButtonDown();
        }
        else
        {
            OnJumpButtonUp();
        }
        if (_isJumpPress && _isReadyToJump && _isGrounded)
        {
            _isReadyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }
    private void Move()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        if (OnSlope() && !_isExitingSlope)
        {
            _rigidBody.AddForce(GetSlopeMoveDirection() * _moveSpeed * 20f, ForceMode.Force);

            if (_rigidBody.velocity.y > 0)
                _rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (_isGrounded)
        {
            _rigidBody.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
            Actions.OnMove(_horizontalInput, _verticalInput);
        }

        else if (!_isGrounded) _rigidBody.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rigidBody.velocity = new Vector3(limitedVel.x, _rigidBody.velocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);
        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    public void OnJumpButtonDown() => _isJumpPress = true;
    public void OnJumpButtonUp() => _isJumpPress = false;
    private void ResetJump() => _isReadyToJump = true;
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal.normalized);
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle != 0;
        }
        return false;
    }
}