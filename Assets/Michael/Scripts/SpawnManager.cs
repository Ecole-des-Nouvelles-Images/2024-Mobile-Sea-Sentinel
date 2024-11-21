using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class BoatEnemy
{
    public GameObject BoatPrefab;
    public int spawnCost;
}
public class SpawnManager : MonoBehaviour
{
    public int currentWave = 0;
    public int WaveValue;
    public List<GameObject> boatsToSpawn ;
    [SerializeField] private GameObject _boatPrefab;
    public List<Transform> _spawnPoints;
    [SerializeField] private List<BoatEnemy> _boats = new List<BoatEnemy>();
    public int spawnIndex;
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
  
    
    
    private void Start() {
      InvokeRepeating(nameof(SpwanBoat),0f,5f);
    }

    private void SpwanBoat() {
        int num = Random.Range(0, _spawnPoints.Count);
        Instantiate(_boatPrefab, _spawnPoints[num].position, Quaternion.identity);
      
    }
    
   
}
