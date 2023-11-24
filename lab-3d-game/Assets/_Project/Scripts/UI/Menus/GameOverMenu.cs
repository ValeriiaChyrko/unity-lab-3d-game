using Platformer._Project.Settings;
using Platformer.MyTools;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class GameOverMenu : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0;
        }
        
        public void RestartLevel()
        {
            Time.timeScale = 1;
            ScenesManager.Instance.LoadCurrentLevel();
        }
    }
}