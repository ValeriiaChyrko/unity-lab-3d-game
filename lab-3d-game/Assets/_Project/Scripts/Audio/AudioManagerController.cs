using System;
using System.Linq;
using UnityEngine;

namespace Platformer._Project.Scripts.Audio
{
    public class AudioManagerController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;

        private void Start()
        {
            AudioManager.Instance.PlayOnBackground(clips.ToList());
        }
    }
}