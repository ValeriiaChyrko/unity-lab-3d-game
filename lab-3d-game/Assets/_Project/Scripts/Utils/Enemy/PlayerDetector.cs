using Platformer._Project.Scripts.Utils.Timer;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils.Enemy
{
    public class PlayerDetector : MonoBehaviour {
        [SerializeField] private float detectionAngle = 60f; // Cone in front of enemy
        [SerializeField] private float detectionRadius = 10f; // Large circle around enemy
        [SerializeField] private float innerDetectionRadius = 5f; // Small circle around enemy
        [SerializeField] private float detectionCooldown = 1f; // Time between detections
        [SerializeField] private float attackRange = 2f; // Distance from enemy to player to attack
        
        public Transform Player { get; private set; }
        public Health PlayerHealth { get; private set; }

        private CountdownTimer _detectionTimer;
        
        private IDetectionStrategy _detectionStrategy;
        
        private void Awake() {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerHealth = Player.GetComponent<Health>();
        }

        private void Start() {
            _detectionTimer = new CountdownTimer(detectionCooldown);
            _detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }
        
        private void Update() => _detectionTimer.Tick(Time.deltaTime);

        public bool CanDetectPlayer() {
            return _detectionTimer.IsRunning || _detectionStrategy.Execute(Player, transform, _detectionTimer);
        }

        public bool CanAttackPlayer() {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }
        
        public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => _detectionStrategy = detectionStrategy;
        
        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            // Draw a spheres for the radii
            var position = transform.position;
            Gizmos.DrawWireSphere(position, detectionRadius);
            Gizmos.DrawWireSphere(position, innerDetectionRadius);

            // Calculate our cone directions
            var forward = transform.forward;
            var forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2, 0) * forward * detectionRadius;
            var backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2, 0) * forward * detectionRadius;

            // Draw lines to represent the cone
            Gizmos.DrawLine(position, position + forwardConeDirection);
            Gizmos.DrawLine(position, position + backwardConeDirection);
        }
    }
}