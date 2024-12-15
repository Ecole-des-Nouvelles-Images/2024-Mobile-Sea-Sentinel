using System;
using System.Collections.Generic;
using DG.Tweening;
using Intégration.Scripts;
using Michael.Scripts.Controller;
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
        public Camera _mainCamera;
        public Canvas _canvas;
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
      
        private void Start()
        {
            IsPaused = false;
            _mainCamera = Camera.main;
            _sfxSlider.value = gameData.SfxVolume;
            _musicSlider.value = gameData.MusicVolume;
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
            _mainMusic.Stop();
            _defeatSound.Play();
            OpenPanel(_gameOverPanel);
        }
        
        [ContextMenu("open shop !")]
        public void OpenShop() {
            OpenPanel(_shopPanel);
        }
 
        public void CloseShop() { 
            ClosePanel(_shopPanel);
        }

        public void ShakeCamera() {
            _mainCamera.transform.DOShakePosition(0.5f,0.25f, 10);
        }

        public void OpenPanel(GameObject panel) {
            
            panel.SetActive(true); 
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.Linear));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(1f, 0.25f)));
            showSequence.Play();
            _openButtonSound.Play();
        }
        public void ClosePanel(GameObject panel)
       {
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.OutBounce));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(0f, 0.25f)));
            showSequence.Play();
            showSequence.OnComplete(()=> panel.SetActive(false)); 
            _closeButtonSound.Play();
        }
        
        public void ButtonFeedback(Button button) {
            
            Sequence feedBackSequence = DOTween.Sequence();
            feedBackSequence.Append(button.transform.DOScale(1.1f, 0.2f).SetEase(Ease.Linear));
            feedBackSequence.Append(button.transform.DOScale(1f, 0.2f).SetEase(Ease.Linear));
            feedBackSequence.Play();
            _buttonSound.Play();
        }
        
        public void SetSfxVolume()
        {
            gameData.SfxVolume = _sfxSlider.value;
            _mixer.SetFloat("Sfx", Mathf.Log10(gameData.SfxVolume) * 20);
        }
        
        public void SetMusicVolume()
        {
            gameData.MusicVolume = _musicSlider.value;
            _mixer.SetFloat("Music", Mathf.Log10(gameData.MusicVolume) * 20);
        }
        
    }
}
