using UnityEngine;
public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 9f;
    [SerializeField] private float _walkSpeed = 9f;
    [SerializeField] private float _runSpeed = 15f;



    [SerializeField] private float _groundDrag = 5;
    [SerializeField] private float _jumpForce = 6.3f;
    [SerializeField] private float _jumpCooldown = 0.25f;
    [SerializeField] private float _airMultiplier = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight = 2;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Transform _orientation;

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle = 0;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _moveDirection;
    private Rigidbody _rigidBody;
    private RaycastHit _slopeHit;

    private bool _isReadyToJump;
    private bool _isGrounded;
    private bool _isJumpPress = false;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
        _isReadyToJump = true;
    }
    private void Start()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);
    }
    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (_horizontalInput != 0 || _verticalInput != 0)
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        MyInput();
        SpeedControl();

        if (_isGrounded) _rigidBody.linearDamping = _groundDrag;
        else _rigidBody.linearDamping = 0;

        Move();
    }
    private void MyInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnJumpButtonDown();
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        }
        else OnJumpButtonUp();


        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(0))
        {
            _moveSpeed = _runSpeed;
        }
        else
        {
            _moveSpeed = _walkSpeed;
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
        if (OnSlope())
        {
            _rigidBody.AddForce(_moveSpeed * 20f * GetSlopeMoveDirection(), ForceMode.Force);

            if (_rigidBody.linearVelocity.y > 0)
                _rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (_isGrounded)
        {
            _rigidBody.AddForce(_moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);
            //   Actions.OnMove(_horizontalInput, _verticalInput);
        }

        else if (!_isGrounded) _rigidBody.AddForce(_airMultiplier * _moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);

        Actions.OnMoveSound(_horizontalInput, _verticalInput, _isGrounded);
        Actions.OnMove(_horizontalInput, _verticalInput);
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new(_rigidBody.linearVelocity.x, 0f, _rigidBody.linearVelocity.z);

        if (flatVel.magnitude > _moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rigidBody.linearVelocity = new Vector3(limitedVel.x, _rigidBody.linearVelocity.y, limitedVel.z);
        }
    }
    private void Jump()
    {
        _rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, 0f, _rigidBody.linearVelocity.z);
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