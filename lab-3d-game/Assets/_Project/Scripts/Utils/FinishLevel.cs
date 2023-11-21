using System.Collections;
using Platformer._Project.Scripts.Audio;
using Platformer._Project.Scripts.Utils.Enemy;
using Platformer._Project.Settings;
using UnityEngine;

namespace Platformer._Project.Scripts.Utils
{
    public class FinishLevel : MonoBehaviour
    {
        [SerializeField] private PlayerDetector playerDetector;
        [SerializeField] private AudioClip onHitAudio;
        
        public FinishLevel(PlayerDetector playerDetector)
        {
            this.playerDetector = playerDetector;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (playerDetector.CanDetectPlayer())
                EnableGameOverMenuWithDelay();
        }

        private void EnableGameOverMenuWithDelay()
        {
            AudioManager.Instance.PlaySound(onHitAudio);
            StartCoroutine(EnableLevelOverMenuCoroutine(1.0f));
        }

        private static IEnumerator EnableLevelOverMenuCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            ScenesManager.Instance.LoadNextLevelMenu();
        }
    }
}