using System.Collections;
using Platformer._Project.Scripts.Audio;
using Platformer._Project.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Platformer._Project.Scripts.UI
{
    public class ScoreUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private AudioClip onHitAudio;

        private void Start() {
            UpdateScore();
        }

        public void UpdateScore() {
            StartCoroutine(UpdateScoreNextFrame());
        }
        
        private IEnumerator UpdateScoreNextFrame() {
            yield return null;
            AudioManager.Instance.PlaySound(onHitAudio);
            scoreText.text = GameManager.Instance.Score.ToString();
        }
    }
}