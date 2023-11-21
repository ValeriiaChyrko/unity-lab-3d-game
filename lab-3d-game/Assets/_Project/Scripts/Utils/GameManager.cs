using Platformer._Project.Scripts.UI.Menus;
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
        
        private void Update()
        {
            if (!UnityEngine.Input.GetKeyDown(KeyCode.Escape)) return;

            PauseMenu.SetState();
        }
        
        public void AddScore(int score) {
            Score += score;
        }
    }
}