using UnityEngine;
using UnityEngine.Audio;

namespace Platformer._Project.Scripts.UI
{
    public class MusicSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        public void ChangeMasterVolume(float volume)
        {
            mixer.GetFloat("Master", out var value);
        }
        
        public void ChangeSoundsVolume()
        {
            
        }
        
        public void ChangeMusicVolume()
        {
            
        }
    }
}
