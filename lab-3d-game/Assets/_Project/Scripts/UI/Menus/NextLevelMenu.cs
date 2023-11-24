using System.IO;
using Platformer._Project.Scripts.Utils;
using Platformer._Project.Scripts.Utils.Entity;
using Platformer._Project.Settings;
using TMPro;
using UnityEngine;

namespace Platformer._Project.Scripts.UI.Menus
{
    public class NextLevelMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI maxScoreText;
        
        private const string PATH = "/player/data.json";

        private int _currentScore;
        private int _maxScore;

        private void Awake()
        {
            Time.timeScale = 0;
            _currentScore = GameManager.Instance.Score;
            _maxScore = GetMaxScore();
            
            scoreText.text = "Набрано балів " + _currentScore;
            maxScoreText.text = "Найкращий результат " + _maxScore;

            _maxScore = _currentScore > _maxScore ? _currentScore : _maxScore;
            WriteToFile();
        }

        private int GetMaxScore()
        {
            var filePath = Application.persistentDataPath + PATH;
            
            if (!File.Exists(filePath)) return 0;
            
            var json = File.ReadAllText(filePath);
            var data = JsonUtility.FromJson<PlayerData>(json);
            return data.maxScore;

        }

        private void WriteToFile()
        {
            var data = new PlayerData
            {
                name = "Any", 
                currentScore = _currentScore,
                maxScore = _maxScore,
                healthRate = Health.CurrentHealth
            };
            var json = JsonUtility.ToJson(data);
            var filePath = Application.persistentDataPath + PATH;
            File.WriteAllText(filePath, json);
        }

        public void OnClickNextLevel()
        {
            ScenesManager.Instance.LoadNextLevel();
        }
    }
}