using Platformer._Project.Scripts.Input;
using UnityEngine;

namespace Platformer._Project.Scripts.StateMachine.PlayerStates
{
    public class AttackState : BaseState {
        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Animator.CrossFade(AttackHash, CrossFadeDuration);
            Player.Attack();
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}