using Platformer._Project.Scripts.Utils.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer._Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyWanderState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Vector3 _startPoint;
        private readonly float _wanderRadius;

        public EnemyWanderState(Enemy enemy, Animator animator, NavMeshAgent agent, float wanderRadius) : base(enemy, animator) {
            _agent = agent;
            _startPoint = enemy.transform.position;
            _wanderRadius = wanderRadius;
        }

        public override void OnEnter() {
            Animator.CrossFade(WalkHash, CrossFadeDuration);
        }

        public override void Update()
        {
            if (!HasReachedDestination()) return;
            var randomDirection = Random.insideUnitSphere * _wanderRadius;
            randomDirection += _startPoint;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, _wanderRadius, 1);
            var finalPosition = hit.position;
                
            _agent.SetDestination(finalPosition);
        }
        
        private bool HasReachedDestination() {
            return !_agent.pathPending
                   && _agent.remainingDistance <= _agent.stoppingDistance
                   && (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
        }
    }
}