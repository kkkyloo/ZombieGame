using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 9f;
    [SerializeField] private float _runSpeed = 15f;
    [SerializeField] private float _groundDrag = 5;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight = 2;
    [SerializeField] private LayerMask _ground;

    [Header("Rolling")]
    [SerializeField] private float _rollSpeed = 12f;
    [SerializeField] private float _rollDuration = 0.5f;
    [SerializeField] private float _rollCooldown = 1f;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _velocity;
    private bool _isGrounded;
    private bool _isRolling = false;
    private bool _canRoll = true;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleInput();
        HandleMovement();
        HandleJump();
        HandleRolling();
    }

    private void HandleInput()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
    }

    private void HandleMovement()
    {
        // Ground check
        _isGrounded = Physics.CheckSphere(transform.position, _playerHeight * 0.5f + 0.2f, _ground);

        Vector3 moveDirection = transform.right * _horizontalInput + transform.forward * _verticalInput;

        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;

        _characterController.Move(moveDirection * speed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void HandleRolling()
    {
        if (_isRolling) return;

        if (Input.GetKeyDown(KeyCode.C) && _canRoll && _horizontalInput != 0 || _verticalInput != 0)
        {
            StartCoroutine(Roll());
        }
    }

    private IEnumerator Roll()
    {
        _isRolling = true;
        _canRoll = false;

        float elapsedTime = 0f;
        Vector3 rollDirection = new Vector3(_horizontalInput, 0, _verticalInput).normalized;

        while (elapsedTime < _rollDuration)
        {
            _characterController.Move(rollDirection * _rollSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _isRolling = false;

        yield return new WaitForSeconds(_rollCooldown);
        _canRoll = true;
    }
}
