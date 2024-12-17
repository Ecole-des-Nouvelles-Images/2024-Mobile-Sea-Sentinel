using System;
using System.Collections.Generic;
using Alexandre;
using DG.Tweening;
using Intégration.Scripts;
using Michael.Scripts.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = Unity.Mathematics.Random;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public static bool IsPaused = false;
        public Canvas _canvas;
        public bool GameIsFinished;
        public int BoatDestoyed;
        [SerializeField] GameData gameData;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private AudioSource _closeButtonSound;
        [SerializeField] private AudioSource _openButtonSound;
        [SerializeField] private AudioSource _buttonSound;
        [SerializeField] private AudioSource _defeatSound;
        [SerializeField] private AudioSource _mainMusic;
        [SerializeField] private float _shakeVibrato;
        [SerializeField] private AudioMixer _mixer;
        [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private TextMeshProUGUI _currentWaveText; 
        [SerializeField] private TextMeshProUGUI _highScoreText;
        [SerializeField] private TextMeshProUGUI _destroyedBoatText;
        [SerializeField] private List<Button> _buyButtons;
        private int _waveHighScore;
        private int _currentWave;
        private Camera _mainCamera;
        private bool _toogleChangeEnable;
        private void Start()
        {
            IsPaused = false;
            InitiateVolumeSlider();
            _toogleChangeEnable = true;
            _mainCamera = Camera.main;
            _waveHighScore =  PlayerPrefs.GetInt("HighWave", 0);
            _highScoreText.text = "Record : Vague " + _waveHighScore;
        }

        void Update()
        {
            if (IsPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        public void Pause()
        {
            IsPaused = !IsPaused;
        }

        public void LoadScene(string title)
        {
            SceneManager.LoadScene(title);
        }

        public void GameOver()
        {
            Pause();
            _mainMusic.Stop();
            _defeatSound.Play();
            OpenPanel(_gameOverPanel);

           _currentWave =  WaveManager.Instance.GetCurrentWave();
           _currentWaveText.text = "Vague atteinte : " + _currentWave;
           _destroyedBoatText.text = "Bateaux detruits : " + BoatDestoyed;
            
            if (_currentWave > _waveHighScore)
            {
                PlayerPrefs.SetInt("HighWave", _currentWave); // Sauvegarder le nouveau record
                _highScoreText.text = "Nouveau Record ! Vague " + _currentWave;
            }
        }
        
        

        [ContextMenu("open shop !")]
        public void OpenShop()
        {
            OpenPanel(_shopPanel);
            
            foreach (var button in _buyButtons) {
                Upgrade.Upgrade upgrade = button.GetComponent<Upgrade.Upgrade>();
                if (PlayerData.Instance.CurrentGold > upgrade.CurrentCost)
                {
                    upgrade.CostText.color = Color.red;
                }
            }
        }

        public void CloseShop()
        {
            ClosePanel(_shopPanel);
        }

        public void ShakeCamera()
        {
            _mainCamera.transform.DOShakePosition(0.5f, 0.25f, 10);
        }

        public void OpenPanel(GameObject panel)
        {

            panel.SetActive(true);
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.Linear));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(1f, 0.25f)));
            showSequence.SetUpdate(true);
            showSequence.Play();
            _openButtonSound.Play();
        }

        public void ClosePanel(GameObject panel)
        {
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBounce));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(0f, 0.25f)));
            showSequence.SetUpdate(true);
            showSequence.Play();
            showSequence.OnComplete(() => panel.SetActive(false));
            _closeButtonSound.Play();
        }

        public void ButtonFeedback(GameObject button)
        {

            Sequence feedBackSequence = DOTween.Sequence();
            feedBackSequence.Append(button.transform.DOScale(1.1f, 0.2f).SetEase(Ease.Linear));
            feedBackSequence.Append(button.transform.DOScale(1f, 0.2f).SetEase(Ease.Linear));
            feedBackSequence.SetUpdate(true);
            feedBackSequence.Play();
            _buttonSound.Play();
        }

        public void SetSfxVolume()
        {
            gameData.SfxVolume = _sfxSlider.value;
            _mixer.SetFloat("Sfx", Mathf.Log10(gameData.SfxVolume) * 20);
            _buttonSound.Play();
        }

        public void SetMusicVolume()
        {
            gameData.MusicVolume = _musicSlider.value;
            _mixer.SetFloat("Music", Mathf.Log10(gameData.MusicVolume) * 20);
            _buttonSound.Play();
        }

        private void UpdateScoreDisplay(int wavesSurvived, int[] highScores)
        {
            // _waveSurvivedText.text = "Waves Survived: " + wavesSurvived;
            //  _highScoresText.text = "High Scores:\n1. " + highScores[0] + "\n2. " + highScores[1] + "\n3. " + highScores[2];
        }

        public void ToggleSfxVolume()
        {
            if (_toogleChangeEnable)
            {
                if (gameData._sfxSliderEnable)
                {
                    _sfxSlider.value = _sfxSlider.minValue;
                    gameData.SfxVolume = _sfxSlider.value;
                    _sfxSlider.interactable = false;
                    _sfxSlider.GetComponentInParent<CanvasGroup>().alpha = 0.5f;
                    gameData._sfxSliderEnable = false;
                }
                else
                {
                    _sfxSlider.value = _sfxSlider.maxValue / 2;
                    gameData.SfxVolume = _sfxSlider.value;
                    _sfxSlider.interactable = true;
                    _sfxSlider.GetComponentInParent<CanvasGroup>().alpha = 1f;
                    gameData._sfxSliderEnable = true;
                }
                _buttonSound.Play();
            }

        }

        public void ToggleMusicVolume()
        {
            if (_toogleChangeEnable)
            {
                if (gameData._musicSliderEnable)
                {
                    _musicSlider.value = _musicSlider.minValue;
                    _musicSlider.interactable = false;
                    _musicSlider.GetComponentInParent<CanvasGroup>().alpha = 0.5f;
                    gameData._musicSliderEnable = false;
                }
                else
                {
                    _musicSlider.value = _musicSlider.maxValue / 2;
                    gameData.MusicVolume = _musicSlider.value;
                    _musicSlider.interactable = true;
                    _musicSlider.GetComponentInParent<CanvasGroup>().alpha = 1;
                    gameData._musicSliderEnable = true;
                }
                _buttonSound.Play();
            }


        }

        private void InitiateVolumeSlider()
        {
            _musicSlider.interactable = gameData._musicSliderEnable;
            _sfxSlider.interactable = gameData._sfxSliderEnable;

            _musicSlider.value = gameData._musicSliderEnable ? gameData.MusicVolume : _musicSlider.minValue;
            _sfxSlider.value = gameData._sfxSliderEnable ? gameData.SfxVolume : _sfxSlider.minValue;
            
            _musicToggle.isOn = gameData._musicSliderEnable;
            _sfxToggle.isOn = gameData._sfxSliderEnable;
            
        }
        
    }
}
