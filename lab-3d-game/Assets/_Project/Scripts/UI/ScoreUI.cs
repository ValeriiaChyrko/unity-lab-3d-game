using System.Collections;
using Platformer._Project.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Platformer._Project.Scripts.UI
{
    public class ScoreUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start() {
            UpdateScore();
        }

        public void UpdateScore() {
            StartCoroutine(UpdateScoreNextFrame());
        }
        
        private IEnumerator UpdateScoreNextFrame() {
            yield return null;
            scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}