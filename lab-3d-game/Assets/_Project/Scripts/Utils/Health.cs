using System.Collections;
using Platformer._Project.EventSystem;
using Platformer._Project.Settings;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils
{
    public class Health : MonoBehaviour {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;

        public static int CurrentHealth { get; private set; }
        
        public bool IsDead => CurrentHealth <= 0;
            
        private void Awake() {
            CurrentHealth = maxHealth;
        }

        private void Start() {
            PublishHealthPercentage();
        }
        
        public void TakeDamage(int damage) {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                EnableGameOverMenuWithDelay();
            
            PublishHealthPercentage();
        }

        private void PublishHealthPercentage() {
            if (playerHealthChannel != null)
                playerHealthChannel.Invoke(CurrentHealth / (float) maxHealth);
        }
        
        private void EnableGameOverMenuWithDelay()
        {
            StartCoroutine(EnableGameOverMenuCoroutine(1.0f));
        }

        private static IEnumerator EnableGameOverMenuCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            ScenesManager.Instance.LoadGameOverScene();
        }
    }
}