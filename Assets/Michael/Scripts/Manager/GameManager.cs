using Alexandre;
using UnityEngine;
using DG.Tweening;
using Michael.Scripts.Controller;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using TMPro;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private TextMeshProUGUI _waveSurvivedText;
        [SerializeField] private TextMeshProUGUI _highScoresText;
        public Camera _mainCamera;
        [SerializeField] private float _shakeVibrato;
        public Canvas _canvas;

        private void Start()
        {
            Time.timeScale = 1;
            _mainCamera = Camera.main;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("Game");
        }

        public void GameOver()
        {
            OpenPanel(_gameOverPanel);
            int wavesSurvived = ScoreManager.Instance.GetWavesSurvived();
            int[] highScores = ScoreManager.Instance.GetHighScores();
            ScoreManager.Instance.ResetWavesSurvived(); // Réinitialiser le nombre de vagues survécues
            ScoreManager.Instance.AddScore(wavesSurvived); // Ajouter le score actuel aux hauts scores
            UpdateScoreDisplay(wavesSurvived, highScores);
        }

        [ContextMenu("open shop !")]
        public void OpenShop()
        {
            OpenPanel(_shopPanel);
        }

        public void CloseShop()
        {
            ClosePanel(_shopPanel);
        }

        public void ShakeCamera()
        {
            _mainCamera.transform.DOShakePosition(0.5f, 0.25f, 10);
        }

        private void OpenPanel(GameObject panel)
        {
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
            showSequence.OnComplete(() => panel.SetActive(false));
        }

        private void UpdateScoreDisplay(int wavesSurvived, int[] highScores)
        {
            _waveSurvivedText.text = "Waves Survived: " + wavesSurvived;
            _highScoresText.text = "High Scores:\n1. " + highScores[0] + "\n2. " + highScores[1] + "\n3. " + highScores[2];
        }
    }
}
