using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Platformer._Project.Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        [SerializeField] private Transform soundContainer;
        [SerializeField] private AudioMixerGroup audioMixer;
        [SerializeField] private AudioSource backgroundMusic;

        private readonly List<AudioSource> _soundSources = new List<AudioSource>();
        private List<AudioClip> _musicClip = new List<AudioClip>();

        private Coroutine _controlMusicRoutine;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySound(AudioClip clip)
        {
            var soundSource = GetFreeSoundSource();
            soundSource.clip = clip;
            soundSource.Play();
        }

        public void PlayOnBackground(List<AudioClip> clips)
        {
            _musicClip = clips;
            backgroundMusic.clip = clips[0];
            backgroundMusic.Play();

            if (_controlMusicRoutine != null)
                StopCoroutine(_controlMusicRoutine);
            _controlMusicRoutine = StartCoroutine(ControlMusicRoutine());
        }

        private IEnumerator ControlMusicRoutine()
        {
            var musicIndex = 0;

            while (true)
            {
                yield return null;

                if (backgroundMusic.isPlaying != false) continue;
                
                musicIndex++;
                if (musicIndex >= _musicClip.Count)
                    musicIndex = 0;

                backgroundMusic.clip = _musicClip[musicIndex];
                backgroundMusic.Play();
            }
        }

        private AudioSource GetFreeSoundSource()
        {
            var freeAudioSource = _soundSources.Find(s => s.isPlaying == false);

            if (freeAudioSource != null)
                return freeAudioSource;

            freeAudioSource = soundContainer.AddComponent<AudioSource>();
            freeAudioSource.outputAudioMixerGroup = audioMixer;
            _soundSources.Add(freeAudioSource);
            return freeAudioSource;
        }
    }
}