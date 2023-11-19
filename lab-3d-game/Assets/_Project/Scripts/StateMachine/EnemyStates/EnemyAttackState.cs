using Platformer._Project.Scripts.Utils.Enemy;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer._Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyAttackState : EnemyBaseState {
        private readonly NavMeshAgent _agent;
        private readonly Transform _player;
        
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform player) : base(enemy, animator) {
            _agent = agent;
            _player = player;
        }
        
        public override void OnEnter() {
            Debug.Log("Attack");
            Animator.CrossFade(AttackHash, CrossFadeDuration);
        }
        
        public override void Update() {
            _agent.SetDestination(_player.position);
            Enemy.Attack();
        }
    }
}