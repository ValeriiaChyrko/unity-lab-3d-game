using UnityEngine;

namespace Platformer._Project.Scripts.Utils
{
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        
        public int Score { get; private set; }
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        
        public void AddScore(int score) {
            Score += score;
        }
    }
}