using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
     public WaveManager waveManager;
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
         waveManager.currentWave++;
         waveManager.StartWave(); // Lance la prochaine vague
     }
}
