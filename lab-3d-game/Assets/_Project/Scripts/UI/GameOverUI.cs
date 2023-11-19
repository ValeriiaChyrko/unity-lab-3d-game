using System.Collections;
using Platformer._Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer._Project.Scripts.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverMenu;

        private void OnEnable()
        {
            Health.OnPlayerDeath += EnableGameOverMenuWithDelay;
        }

        private void OnDisable()
        {
            Health.OnPlayerDeath -= EnableGameOverMenuWithDelay;
        }

        private void EnableGameOverMenuWithDelay()
        {
            StartCoroutine(EnableGameOverMenuCoroutine(1.0f));
        }

        private IEnumerator EnableGameOverMenuCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            gameOverMenu.SetActive(true);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}