using System.Linq;
using Michael.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace Alexandre
{
    public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
    {
        
        private int _wavesSurvived = 0;
        private int[] _highScores = new int[3];

        private void Start()
        {
            _wavesSurvived = PlayerPrefs.GetInt("WavesSurvived", 0);
            LoadHighScores();
        }

        public void IncrementWavesSurvived()
        {
            _wavesSurvived++;
            PlayerPrefs.SetInt("WavesSurvived", _wavesSurvived);
        }

        public void ResetWavesSurvived()
        {
            _wavesSurvived = 0;
            PlayerPrefs.SetInt("WavesSurvived", _wavesSurvived);
        }

        public void AddScore(int score)
        {
            _highScores = _highScores.Concat(new[] { score }).OrderByDescending(s => s).Take(3).ToArray();
            SaveHighScores();
        }

        private void LoadHighScores()
        {
            for (int i = 0; i < 3; i++)
            {
                _highScores[i] = PlayerPrefs.GetInt("HighScore" + i, 0);
            }
        }

        private void SaveHighScores()
        {
            for (int i = 0; i < 3; i++)
            {
                PlayerPrefs.SetInt("HighScore" + i, _highScores[i]);
            }
        }


        

        public int GetWavesSurvived()
        {
            return _wavesSurvived;
        }

        public int[] GetHighScores()
        {
            return _highScores;
        }
        
        
    }
}
