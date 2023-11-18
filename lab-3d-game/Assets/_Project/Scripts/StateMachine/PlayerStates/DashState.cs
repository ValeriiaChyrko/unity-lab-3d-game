using Platformer._Project.Scripts.Input;
using UnityEngine;

namespace Platformer._Project.Scripts.StateMachine.PlayerStates
{
    public class DashState : BaseState {
        public DashState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Animator.CrossFade(DashHash, CrossFadeDuration);
        }

        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}