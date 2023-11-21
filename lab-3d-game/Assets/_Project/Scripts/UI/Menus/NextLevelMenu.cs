using Platformer._Project.Scripts.Utils;
using Platformer._Project.Settings;
using TMPro;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class NextLevelMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        public void OnClickNextLevel()
        {
            scoreText.text = GameManager.Instance.Score.ToString();
            ScenesManager.Instance.LoadNextLevel();
        }
    }
}