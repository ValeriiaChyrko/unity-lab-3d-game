using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float acceleration = 5.0f;
    private const float JumpHeight = 1.5f;
    private const float GravityValue = -9.81f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private bool _isSprintingWhileGrounded;

    private float _horizontal;
    private float _vertical;
    private bool _jump;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
            _playerVelocity.y = 0f;

        var move = new Vector3(_horizontal, 0, _vertical);
        var moveDirection = transform.TransformDirection(move);

        _isSprintingWhileGrounded = Input.GetKey(KeyCode.LeftShift) && _groundedPlayer;
        var currentSpeed = _isSprintingWhileGrounded ? playerSpeed * acceleration : playerSpeed;

        if (_jump && _groundedPlayer)
            _playerVelocity.y = Mathf.Sqrt(2.0f * JumpHeight * -GravityValue);

        _playerVelocity.y += GravityValue * Time.deltaTime;

        _controller.Move(moveDirection * (Time.deltaTime * currentSpeed) + _playerVelocity * Time.deltaTime);
    }
}
