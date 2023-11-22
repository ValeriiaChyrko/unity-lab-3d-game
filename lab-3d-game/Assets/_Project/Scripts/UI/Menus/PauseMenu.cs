using KBCore.Refs;
using Platformer._Project.Settings;
using Platformer.MyTools;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class PauseMenu: MonoBehaviour
    {
        private static bool _isPaused;
        
        public static void SetState()
        {
            _isPaused = !_isPaused;
            
            if (_isPaused)
                ActivateMenu();
            else
                DeactivateMenu();
        }

        private static void ActivateMenu()
        {
            Time.timeScale = 0;
            ScenesManager.Instance.LoadPauseScene();
        }
        
        public static void DeactivateMenu()
        {
            _isPaused = false;
            Time.timeScale = 1;
            ScenesManager.Instance.UnloadPauseScene();
        }
        
        public void OnClickSettings()
        {
            DeactivateMenu();
            ScenesManager.Instance.LoadSettingsMenu();
        }
        
        public void OnClickMainMenu()
        {
            DeactivateMenu();
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}