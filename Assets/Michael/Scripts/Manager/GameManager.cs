using UnityEngine;

namespace Michael.Scripts.Manager
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
     
        public GameObject shopUI; // Interface du shop
 
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
    }
}
