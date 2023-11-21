using System;
using System.Collections;
using Platformer._Project.EventSystem;
using Platformer.MyTools;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils
{
    public class Health : MonoBehaviour {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;

        private int _currentHealth;
        
        public bool IsDead => _currentHealth <= 0;
            
        private void Awake() {
            _currentHealth = maxHealth;
        }

        private void Start() {
            PublishHealthPercentage();
        }
        
        public void TakeDamage(int damage) {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
                EnableGameOverMenuWithDelay();
            
            PublishHealthPercentage();
        }

        private void PublishHealthPercentage() {
            if (playerHealthChannel != null)
                playerHealthChannel.Invoke(_currentHealth / (float) maxHealth);
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