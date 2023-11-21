using Platformer._Project.Settings;
using Platformer.MyTools;
using UnityEngine;
using UnityEngine.Audio;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class SettingsMenu: MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void ChangeMasterVolume(float volume)
        {
            audioMixer.SetFloat("master-volume", Mathf.Log10(volume) * 20f);
        }
        
        public void ChangeSoundsVolume(float volume)
        {
            audioMixer.SetFloat("sound-volume", Mathf.Log10(volume) * 20f);
        }
        
        public void ChangeMusicVolume(float volume)
        {
            audioMixer.SetFloat("music-volume", Mathf.Log10(volume) * 20f);
        }
        
        public static void DeactivateMenu()
        {
            ScenesManager.Instance.UnloadSettingsMenu();
        }
    }
}