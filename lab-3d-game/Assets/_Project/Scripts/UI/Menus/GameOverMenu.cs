using Platformer._Project.Settings;
using Platformer.MyTools;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class GameOverMenu : MonoBehaviour
    {
        public void RestartLevel()
        {
            ScenesManager.Instance.LoadCurrentLevel();
        }
    }
}