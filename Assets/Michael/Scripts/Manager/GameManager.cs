using System;
using DG.Tweening;
using Michael.Scripts.Controller;
using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        
        public GameObject shopUI; // Interface du shop
        public PlayerData playerData;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _shakeVibrato;
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            
        }

        public void OpenShop()
        {
            shopUI.SetActive(true); // Affiche l'interface du shop
            Time.timeScale = 0; // Met le jeu en pause
        }
 
        public void CloseShop()
        {
            shopUI.SetActive(false); // Cache l'interface du shop
            Time.timeScale = 1; // Reprend le jeu
            // waveManager.StartWave(); // Lance la prochaine vague
        }

        public void ShakeCamera()
        {
            _mainCamera.transform.DOShakePosition(0.5f,0.25f, 10);
        }
    }
}
