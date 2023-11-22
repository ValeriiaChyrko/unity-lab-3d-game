using System;
using Platformer._Project.Settings;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class GameFinishMenu : MonoBehaviour
    {
        private void Awake()
        {
            Time.timeScale = 0;
        }

        public void OnClickMainMenu()
        {
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}