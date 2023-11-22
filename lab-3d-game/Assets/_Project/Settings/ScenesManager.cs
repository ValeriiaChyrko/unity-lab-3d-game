using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer._Project.Settings
{
    public class ScenesManager : MonoBehaviour
    {
        public static ScenesManager Instance { get; private set; }

        private const string MAIN_MENU_SCENE_KEY = "MainMenu";
        private const string SETTINGS_SCENE_KEY = "MusicSettingsMenu";
        private const string PAUSE_SCENE_KEY = "PauseMenu";
        private const string GAME_OVER_SCENE_KEY = "GameOverMenu";
        private const string GAME_FINISH_SCENE_KEY = "GameFinishMenu";
        private const string LEVEL_FINISH_SCENE_KEY = "LevelFinishMenu";

        private const string GAME_PLAY_SCENE_KEY = "GamePlay";
        private string[] LEVELS_SCENE_KEYS = new string[] { "Level 01", "Level 02" };

        private string currentLevel;

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

        private void LoadScene(string sceneKey, bool additive = false)
        {
            Time.timeScale = 0;
            if (additive)
                SceneManager.LoadScene(sceneKey, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(sceneKey);
        }

        public void LoadMainMenu() => LoadScene(MAIN_MENU_SCENE_KEY, true);

        public void UnloadMainMenu()
        {
            SceneManager.UnloadSceneAsync(MAIN_MENU_SCENE_KEY);
            Time.timeScale = SceneManager.loadedSceneCount > 3 ? 0 : 1;
        }

        public void LoadSettingsMenu() => LoadScene(SETTINGS_SCENE_KEY, true);

        public void UnloadSettingsMenu()
        {
            SceneManager.UnloadSceneAsync(SETTINGS_SCENE_KEY);
            Time.timeScale = SceneManager.loadedSceneCount > 3 ? 0 : 1;
        }

        public void LoadGameOverScene() => LoadScene(GAME_OVER_SCENE_KEY, true);

        public void LoadPauseScene() => LoadScene(PAUSE_SCENE_KEY, true);

        public void UnloadPauseScene()
        {
            SceneManager.UnloadSceneAsync(PAUSE_SCENE_KEY);
            Time.timeScale = SceneManager.loadedSceneCount > 3 ? 0 : 1;
        }

        public void LoadNextLevelMenu() => LoadScene(LEVEL_FINISH_SCENE_KEY);

        public void LoadFirstLevel()
        {
            currentLevel = currentLevel ?? LEVELS_SCENE_KEYS[0];
            SceneManager.LoadScene(currentLevel);
            SceneManager.LoadScene(GAME_PLAY_SCENE_KEY, LoadSceneMode.Additive);
            Time.timeScale = 1;
        }

        public void LoadCurrentLevel()
        {
            currentLevel = currentLevel ?? LEVELS_SCENE_KEYS[0];
            SceneManager.LoadScene(currentLevel);
            SceneManager.LoadScene(GAME_PLAY_SCENE_KEY, LoadSceneMode.Additive);
            Time.timeScale = 1;
        }

        public void LoadNextLevel()
        {
            var currentSceneIndex = Array.IndexOf(LEVELS_SCENE_KEYS, currentLevel);
            if (currentSceneIndex < LEVELS_SCENE_KEYS.Length - 1)
            {
                SceneManager.LoadScene(LEVELS_SCENE_KEYS[currentSceneIndex + 1]);
                SceneManager.LoadScene(GAME_PLAY_SCENE_KEY, LoadSceneMode.Additive);
                currentLevel = LEVELS_SCENE_KEYS[currentSceneIndex + 1];
                Time.timeScale = 1;
            }
            else
            {
                SceneManager.LoadScene(GAME_FINISH_SCENE_KEY);
                Time.timeScale = 0;
            }
        }
    }
}