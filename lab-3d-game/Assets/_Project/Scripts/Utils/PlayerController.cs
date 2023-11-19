using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using Platformer._Project.Scripts.StateMachine;
using Platformer._Project.Scripts.StateMachine.PlayerStates;
using Platformer._Project.Scripts.Utils;
using Platformer._Project.Scripts.Utils.Timer;
using UnityEngine;

namespace Platformer._Project.Scripts.Input
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] private Rigidbody rb;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Self] private GroundChecker groundChecker;
        [SerializeField, Anywhere] private InputReader input;
        [SerializeField, Anywhere]private  CinemachineFreeLook freeLookVCam;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpCooldown;
        [SerializeField] private float gravityMultiplier = 3f;
        
        [Header("Dash Settings")]
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float dashDuration = 1f;
        [SerializeField] private float dashCooldown = 2f;
        
        [Header("Attack Settings")]
        [SerializeField] private float attackCooldown = 0.5f;
        [SerializeField] private float attackDistance = 1f;
        [SerializeField] private int attackDamage = 10;

        private const float ZeroF = 0f;
        
        private Transform _mainCam;

        private float _jumpVelocity;
        private float _currentSpeed;
        private float _velocity;
        private float _dashVelocity = 1f;

        private Vector3 _movement;
        
        private List<Timer> _timers;
        private CountdownTimer _jumpTimer;
        private CountdownTimer _jumpCooldownTimer;
        private CountdownTimer _dashTimer;
        private CountdownTimer _dashCooldownTimer;
        private CountdownTimer _attackTimer;


        private StateMachine.StateMachine _stateMachine;
        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");
        
        private void Start() => input.EnablePlayerActions();
        private void OnEnable() {
            input.Jump += OnJump;
            input.Dash += OnDash;
            input.Attack += OnAttack;
        }
        private void OnDisable() {
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            input.Attack -= OnAttack;
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
            SetupStateMachine();
        }

        private void SetupStateMachine() {
            // State Machine
            _stateMachine = new StateMachine.StateMachine();

            // Declare states
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            var attackState = new AttackState(this, animator);
            var dieState = new DieState(this, animator); 

            // Define transitions
            At(locomotionState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));
            At(locomotionState, dashState, new FuncPredicate(() => _dashTimer.IsRunning));
            At(locomotionState, attackState, new FuncPredicate(() => _attackTimer.IsRunning));
            At(locomotionState, dieState, new FuncPredicate(() => GetComponent<Health>().IsDead));
            At(attackState, locomotionState, new FuncPredicate(() => !_attackTimer.IsRunning));
            
            Any(locomotionState, new FuncPredicate(ReturnToLocomotionState));
    
            // Set initial state
            _stateMachine.SetState(locomotionState);
        }

        
        private bool ReturnToLocomotionState() {
            return groundChecker.IsGrounded 
                   && !_attackTimer.IsRunning 
                   && !_jumpTimer.IsRunning 
                   && !_dashTimer.IsRunning;
        }
        
        private void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
        private void Update()
        {
            _movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            _stateMachine.Update();
            
            HandleTimers();
            HandleAnimator();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }

        private void HandleAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        public void HandleMovement()
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

        private void HandleHorizontalMovement(Vector3 adjustedDirection) {
            // Move the player
            Vector3 velocity = adjustedDirection * (moveSpeed * _dashVelocity * Time.fixedDeltaTime);
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        
        private void HandleRotation(Vector3 adjustedDirection) 
        {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        public void HandleJump() {
            switch (_jumpTimer.IsRunning)
            {
                // If not jumping and grounded, keep jump velocity at 0
                case false when groundChecker.IsGrounded:
                    _jumpVelocity = ZeroF;
                    return;
                case false:
                    // Gravity takes over
                    _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
                    break;
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
        
        private void OnDash(bool performed)
        {
            switch (performed)
            {
                case true when !_dashTimer.IsRunning && !_dashCooldownTimer.IsRunning:
                    _dashTimer.Start();
                    break;
                case false when _dashTimer.IsRunning:
                    _dashTimer.Stop();
                    break;
            }
        }
        
        private void OnAttack() {
            if (!_attackTimer.IsRunning) {
                _attackTimer.Start();
            }
        }
        
        public void Attack() {
            var attackPos = transform.position + transform.forward;
            var hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);
            
            foreach (var enemy in hitEnemies) {
                Debug.Log(enemy.name);
                if (enemy.CompareTag("Enemy")) {
                    enemy.GetComponent<Health>().TakeDamage(attackDamage);
                }
            }
        }
        
        private void SetupTimers() {
            // Setup timers
            _jumpTimer = new CountdownTimer(jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            _jumpTimer.OnTimerStart += () => _jumpVelocity = jumpForce;
            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();
            
            _dashTimer = new CountdownTimer(dashDuration);
            _dashCooldownTimer = new CountdownTimer(dashCooldown);

            _dashTimer.OnTimerStart += () => _dashVelocity = dashForce;
            _dashTimer.OnTimerStop += () => {
                _dashVelocity = 1f;
                _dashCooldownTimer.Start();
            };
            
            _attackTimer = new CountdownTimer(attackCooldown);

            _timers = new List<Timer>(5) { _jumpTimer, _jumpCooldownTimer, _dashTimer, _dashCooldownTimer, _attackTimer };
        }
    }
}