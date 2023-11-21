using KBCore.Refs;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils.Enemy
{
    public class DeadZone : MonoBehaviour
    {
        [SerializeField, Self] private PlayerDetector playerDetector;
        
        public DeadZone(PlayerDetector playerDetector)
        {
            this.playerDetector = playerDetector;
        }
        
        private void OnValidate() => this.ValidateRefs();
        private void OnTriggerEnter(Collider collision)
        {
            if (playerDetector.CanDetectPlayer())
                playerDetector.PlayerHealth.TakeDamage(100);
        }
    }
}
