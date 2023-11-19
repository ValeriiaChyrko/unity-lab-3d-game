using Platformer._Project.Scripts.Utils.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer._Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyChaseState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Transform _player;
        
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            _agent = agent;
            _player = player;
        }
        
        public override void OnEnter() {
            Animator.CrossFade(RunHash, CrossFadeDuration);
        }
        
        public override void Update() {
            _agent.SetDestination(_player.position);
        }
    }
}