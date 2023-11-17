using Cinemachine;
using KBCore.Refs;
using UnityEngine;

namespace Platformer._Project.Scripts.Input
{
    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Self] private CharacterController controller;
        [SerializeField, Self] private Animator animator;
        [SerializeField, Anywhere]private  CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] private InputReader input;
        
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;

        private const float ZeroF = 0f;
        
        private Transform _mainCam;
        
        private float _currentSpeed;
        private float _velocity;

        private Vector3 _movement;
        
        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");
        private void Start() => input.EnablePlayerActions();
        
        void Awake() {
            if (Camera.main != null) _mainCam = Camera.main.transform;
            var target = transform;
            freeLookVCam.Follow = target;
            freeLookVCam.LookAt = target;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(target, target.position - freeLookVCam.transform.position - Vector3.forward);
        }
        
        private void Update() {
            HandleMovement();
            HandleAnimator();
        }

        private void HandleAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        private void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            // Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * movementDirection;
            
            if (adjustedDirection.magnitude > ZeroF) {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            } else {
                SmoothSpeed(ZeroF);
            }
        }

        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }
        private void HandleRotation(Vector3 adjustedDirection) 
        {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void SmoothSpeed(float value) 
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }
    }
}