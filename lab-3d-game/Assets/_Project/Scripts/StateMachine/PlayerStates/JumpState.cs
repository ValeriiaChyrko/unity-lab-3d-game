using Platformer._Project.Scripts.Input;
using UnityEngine;

namespace Platformer._Project.Scripts.StateMachine.PlayerStates
{
    public class JumpState : BaseState {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Animator.CrossFade(JumpHash, CrossFadeDuration);
        }

        public override void FixedUpdate() {
            Player.HandleJump();
            Player.HandleMovement();
        }
    }
}