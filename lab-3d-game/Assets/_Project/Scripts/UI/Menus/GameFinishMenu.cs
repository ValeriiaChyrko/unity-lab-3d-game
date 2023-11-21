using Platformer._Project.Settings;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class GameFinishMenu : MonoBehaviour
    {
        public void OnClickMainMenu()
        {
            ScenesManager.Instance.LoadMainMenu();
        }
    }
}