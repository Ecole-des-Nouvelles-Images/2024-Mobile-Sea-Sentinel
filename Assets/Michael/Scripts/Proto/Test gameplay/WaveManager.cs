using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
       public int currentWave = 1;
       public List<GameObject> enemyPrefabs; // Bateaux (petits, moyens, gros)
       public Transform[] spawnPoints; // Points où les bateaux apparaissent
       public int enemiesRemaining = 0; // Ennemis en vie ou non enfuis
       public GameManager gameManager; // Référence au GameManager
   
       void Start()
       {
           StartWave();
       }
   
       public void StartWave()
       {
           StartCoroutine(SpawnWave());
       }
   
       IEnumerator SpawnWave()
       {
           int numEnemies = currentWave * 3; // Exemple : Nombre d'ennemis augmente avec la vague
           enemiesRemaining = numEnemies;
   
           for (int i = 0; i < numEnemies; i++)
           {
               SpawnEnemy();
               yield return new WaitForSeconds(1.5f); // Temps entre les apparitions des bateaux
           }
       }
   
       void SpawnEnemy()
       {
           int enemyType = Random.Range(0, enemyPrefabs.Count); // Sélection aléatoire du type de bateau
           Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
           Instantiate(enemyPrefabs[enemyType], spawnPoint.position, spawnPoint.rotation);
       }
   
       public void EnemyDefeated()
       {
           enemiesRemaining--;
           if (enemiesRemaining <= 0)
           {
               EndWave();
           }
       }
   
       void EndWave()
       {
           Debug.Log("Wave Completed!");
           gameManager.OpenShop(); // Ouvre le shop via le GameManager
       }
}
