using System.Collections;
using UnityEngine;
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
    private void FixedUpdate()
    {
        SpeedControl();

        if (_isRolling)
        {
            IsRunning = false;

            _rigidBody.linearDamping = 0;

            return;
        }

        if (Input.GetKey(KeyCode.LeftControl) && !_isRolling) Chrouching();
        else UnChrouch();



        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.C) && !_isRolling && !Input.GetKey(KeyCode.LeftControl))
        {
            if (_horizontalInput != 0 || _verticalInput != 0)
            {
                rolling = true;

                _rigidBody.linearDamping = 0;

                StartCoroutine(Roll());
                return;
            }
        }


        if (_horizontalInput != 0 || _verticalInput != 0)
        {
            moving = true;
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _ground);

        }
        else
        {
            moving = false;

        }


        if (Input.GetKey(KeyCode.LeftShift) && _horizontalInput != 0 || Input.GetKey(KeyCode.LeftShift) && _verticalInput != 0)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                IsRunning = true;

            }
            else
            {
                IsRunning = false;

            }
        }
        else
        {
            IsRunning = false;
        }










        MyInput();
        SpeedControl();

        if (_isGrounded) _rigidBody.linearDamping = _groundDrag;
        else _rigidBody.linearDamping = 0;

        Move();
    }


    private void Chrouching()
    {
        _walkSpeed = 4;

        IsRunning = false;
        _capsuleCollider.height = 1f;
    }
    private void UnChrouch()
    {
        _walkSpeed = 9;

        _capsuleCollider.height = 2f;
    }





    private IEnumerator Roll()
    {
        _canRoll = false; 
        _isRolling = true;

        _capsuleCollider.height = 1f; 

        _moveDirection = _orientation.forward;

        float startTime = Time.time; 

        while (Time.time - startTime < _rollDuration)
        {

            _rigidBody.AddForce(_rollSpeed  * _moveDirection, ForceMode.Force);
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