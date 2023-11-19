using System;
using Platformer._Project.EventSystem;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils
{
    public class Health : MonoBehaviour {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;

        private int _currentHealth;
        
        public bool IsDead => _currentHealth <= 0;
        public static event Action OnPlayerDeath;
            
        private void Awake() {
            _currentHealth = maxHealth;
        }

        private void Start() {
            PublishHealthPercentage();
        }
        
        public void TakeDamage(int damage) {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
                OnPlayerDeath?.Invoke();
            
            PublishHealthPercentage();
        }

        private void PublishHealthPercentage() {
            if (playerHealthChannel != null)
                playerHealthChannel.Invoke(_currentHealth / (float) maxHealth);
        }
    }
}