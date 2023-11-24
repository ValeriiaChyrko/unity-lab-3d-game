using Platformer._Project.Settings;
using Platformer.MyTools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0;
        }
        
        public void OnClickStart()
        {
            ScenesManager.Instance.LoadFirstLevel();
        }

        public void OnClickContinue()
        {
            Time.timeScale = 1;
            if (SceneManager.sceneCount > 1)
                ScenesManager.Instance.UnloadMainMenu();
            else
                OnClickStart();
        }

        public void OnClickSettings()
        {
            ScenesManager.Instance.LoadSettingsMenu();
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }
    }
}