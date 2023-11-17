using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using Platformer._Project.Scripts.Utils.Timer;
using UnityEngine;

namespace Platformer._Project.Scripts.Input
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] private Rigidbody rb;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Anywhere] private InputReader input;
        [SerializeField, Anywhere]private  CinemachineFreeLook freeLookVCam;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpMaxHeight = 2f;
        [SerializeField] private float jumpCooldown = 0f;
        [SerializeField] private float gravityMultiplier = 3f;

        private const float ZeroF = 0f;
        
        private Transform _mainCam;

        private float _jumpVelocity;
        private float _currentSpeed;
        private float _velocity;

        private Vector3 _movement;
        
        private List<Timer> _timers;
        private CountdownTimer _jumpTimer;
        private CountdownTimer _jumpCooldownTimer;
        
        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");
        
        private void Start() => input.EnablePlayerActions();
        private void OnEnable() {
            input.Jump += OnJump;
        }
        private void OnDisable() {
            input.Jump -= OnJump;
        }
        
        private void Awake() {
            if (Camera.main != null) _mainCam = Camera.main.transform;
            var target = transform;
            freeLookVCam.Follow = target;
            freeLookVCam.LookAt = target;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
            
            rb.freezeRotation = true;
            
            SetupTimers();
        }
        
        private void Update()
        {
            _movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            HandleAnimator();
            HandleTimers();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        private void HandleMovement()
        {
            // Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * _movement;
            
            if (adjustedDirection.magnitude > ZeroF) {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            } else {
                SmoothSpeed(ZeroF);
                
                // Reset horizontal velocity for a snappy stop
                rb.velocity = new Vector3(ZeroF, rb.velocity.y, ZeroF);
            }
        }

        void HandleHorizontalMovement(Vector3 adjustedDirection) {
            // Move the player
            Vector3 velocity = adjustedDirection * (moveSpeed * Time.fixedDeltaTime);
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        
        private void HandleRotation(Vector3 adjustedDirection) 
        {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        public void HandleJump() {
            // If not jumping and grounded, keep jump velocity at 0 if (!jumpTimer.IsRunning && groundChecker.IsGrounded) { jumpVelocity = ZeroF;
            if (!_jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpVelocity = ZeroF;
                _jumpTimer.Stop();
                return;
            }
            
            // If jumping or falling calculate velocity
            if (_jumpTimer.IsRunning) 
            {
                // Progress point for initial burst of velocity
                var launchPoint = 0.9f;
                if (_jumpTimer. Progress> launchPoint) 
                {
                    // Calculate the velocity required to reach the jump height using physics equations v = sqrt(2gh) jumpVelocity = Mathf. Sqrt(f: 2*jumpMaxHeight Mathf. Abs (Physics.gravity.y));
                    _jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                } 
                else 
                {
                    // Gradually apply less velocity as the jump progresses
                    _jumpVelocity += (1 - _jumpTimer.Progress)* jumpForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                // Gravity takes over
                _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
            }
            
            // Apply velocity
            var velocity = rb.velocity;
            velocity = new Vector3(velocity.x, _jumpVelocity, velocity.z);
            rb.velocity = velocity;
        }
        
        void HandleTimers() {
            foreach (var timer in _timers) {
                timer.Tick(Time.deltaTime);
            }
        }

        private void SmoothSpeed(float value) 
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }
        
        private void OnJump(bool performed)
        {
            switch (performed)
            {
                case true when !_jumpTimer.IsRunning && !_jumpCooldownTimer.IsRunning && groundChecker.IsGrounded:
                    _jumpTimer.Start();
                    break;
                case false when _jumpTimer.IsRunning:
                    _jumpTimer.Stop();
                    break;
            }
        }
        
        void SetupTimers() {
            // Setup timers
            _jumpTimer = new CountdownTimer(jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            _timers = new List<Timer>(2) { _jumpTimer, _jumpCooldownTimer };
            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();

        }
    }
}