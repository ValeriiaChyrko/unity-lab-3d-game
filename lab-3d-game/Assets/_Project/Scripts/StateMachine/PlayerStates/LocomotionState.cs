using Platformer._Project.Scripts.Input;
using UnityEngine;

namespace Platformer._Project.Scripts.StateMachine.PlayerStates
{
    public class LocomotionState : BaseState {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator) { }
        
        public override void OnEnter() {
            Debug.Log("LocomotionState.Enter");
            Animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }
        
        public override void FixedUpdate() {
            Player.HandleMovement();
        }
    }
}