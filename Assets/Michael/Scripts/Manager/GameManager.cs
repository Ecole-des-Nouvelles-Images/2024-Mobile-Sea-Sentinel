using System;
using DG.Tweening;
using Intégration.Scripts;
using Michael.Scripts.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    { 
        [SerializeField] GameData gameData;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _musicSlider;
        public Camera _mainCamera;
        [SerializeField] private float _shakeVibrato;
        public Canvas _canvas;
        private void Start()
        {
            Time.timeScale = 1;
           //_sfxSlider.value = gameData.SfxVolume;
         //_musicSlider.value = gameData.MusicVolume;
            _mainCamera = Camera.main;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void GameOver()
        {
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

        private void OpenPanel(GameObject panel) {
            
            panel.SetActive(true); 
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(1f, 0.5f)));
            showSequence.Play();
        }
        private void ClosePanel(GameObject panel)
       {
            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(panel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
            showSequence.Join((panel.GetComponent<CanvasGroup>().DOFade(0f, 0.5f)));
            showSequence.Play();
            showSequence.OnComplete(()=> panel.SetActive(false)); 
        }
        
        
        
        
    }
}
