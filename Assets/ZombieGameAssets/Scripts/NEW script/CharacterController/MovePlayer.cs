using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
public class MovePlayer : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 9f;
    public static float _walkSpeed = 9f;
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


    public static bool IsRunning = false;

    public static bool moving = false;

    public static bool rolling = false;



    [SerializeField] private float _rollSpeed = 12f; // Скорость подката
    [SerializeField] private float _rollDuration = 0.5f; // Длительность подката
    [SerializeField] private float _rollCooldown = 1f; // Время восстановления подката
    [SerializeField] private CapsuleCollider _capsuleCollider;

    private bool _isRolling = false;
    private bool _canRoll = true;







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

    bool crouch = false;

    private Coroutine coroutine;

    private void FixedUpdate()
    {
        SpeedControl();
        MyInput();

        if (_isRolling)
        {
            IsRunning = false;
            return;
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        bool isMoving = _horizontalInput != 0 || _verticalInput != 0;
        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
        bool isControlPressed = Input.GetKey(KeyCode.LeftControl);

        if (isControlPressed && !_isRolling)
        {
            Chrouching();
        }
        else
        {
            UnChrouch();
        }

        if (Input.GetKeyDown(KeyCode.C) && !_isRolling && !crouch)
        {
            rolling = true;
            _rigidBody.linearDamping = 0;
            coroutine = StartCoroutine(Roll());
            return;
        }

        moving = isMoving;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        if (isShiftPressed && isMoving && !isControlPressed)
        {
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }

        SpeedControl();

        _rigidBody.linearDamping = _isGrounded ? _groundDrag : 0;

        Move();
    }


    private void Chrouching()
    {
        crouch = true;
        _walkSpeed = 4;

        IsRunning = false;
        _capsuleCollider.height = 1f;
    }
    private void UnChrouch()
    {
        crouch = false;
        _walkSpeed = 9;

        _capsuleCollider.height = 2f;
    }


    public float maxrollDuration = 15f;


    private IEnumerator Roll()
    {
        _canRoll = false;
        _isRolling = true;

        _capsuleCollider.height = 1f;
        _moveDirection = _orientation.forward;

        float startTime = Time.time;
        float startTime2 = Time.time;

        while (Time.time - startTime < _rollDuration && maxrollDuration > Time.time - startTime2)
        {

            if (_isJumpPress)
            {
                startTime = Time.time;
                _moveDirection = _orientation.forward;

            }

            _rigidBody.AddForce(_rollSpeed * _moveDirection, ForceMode.Force);

            if (Physics.Raycast(transform.position, _moveDirection, out RaycastHit hit, 1f))
            {
                if (hit.collider != null)
                {
                    break;
                }
            }

            yield return null;
        }

        _capsuleCollider.height = 2f;

        _isRolling = false;
        rolling = false;
        yield return new WaitForSeconds(_rollCooldown);
        _canRoll = true;
    }





    private void MyInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            OnJumpButtonDown();
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        }
        else OnJumpButtonUp();


        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetMouseButton(0) && !Input.GetKey(KeyCode.LeftControl))
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
        _moveDirection = (_orientation.forward * _verticalInput + _orientation.right * _horizontalInput).normalized;

        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeMoveDirection();
            _rigidBody.AddForce(_moveSpeed * 20f * slopeDirection, ForceMode.Force);

            if (_rigidBody.linearVelocity.y > 0) 
            {
                _rigidBody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (_isGrounded)
        {
            _rigidBody.AddForce(_moveSpeed * 10f * _moveDirection, ForceMode.Force);
        }
        else
        {
            _rigidBody.AddForce(_airMultiplier * _moveSpeed * 10f * _moveDirection, ForceMode.Force);
        }

   
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
        if (_moveDirection.sqrMagnitude == 0) return false;

        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f, _ground))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < _maxSlopeAngle && angle > 0; 
        }
        return false;
    }
}