using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Michael.Scripts
{
    [Serializable]
    public class BoatEnemy
    {
        public GameObject BoatPrefab;
        public int spawnCost;
    }
    public class SpawnManager : MonoBehaviour
    {
        public GameObject _boatPrefab;
        [SerializeField] private List<BoatEnemy> _boats = new List<BoatEnemy>();
        public int currentWave = 0;
        public int WaveValue;
        public List<GameObject> boatsToSpawn ;
    
        public List<Transform> _spawnPoints;
        public int spawnIndex;
    
        public int waveDuration;
        private float waveTimer;
        private float spawnInterval;
        private float spawnTimer;
  
        [SerializeField] private List<GameObject> _spawnedBoats = new List<GameObject>();
        private void Start() {
            InvokeRepeating(nameof(SpwanBoat),0f,5f);
           // GenerateWave();
        }

          private void SpwanBoat() {
        int num = Random.Range(0, _spawnPoints.Count);
        Instantiate(_boatPrefab, _spawnPoints[num].position, Quaternion.identity);
      
    }

       /* private void FixedUpdate()
        {
            if (spawnTimer >= 0 )
            {
                if (boatsToSpawn.Count >0)
                {
                    GameObject boat = Instantiate(boatsToSpawn[0],_spawnPoints[spawnIndex].position, Quaternion.identity);
              
                    boatsToSpawn.RemoveAt(0);
                    _spawnedBoats.Add(boat);
                    spawnTimer = spawnInterval;

                    if (spawnIndex + 1 <= _spawnPoints.Count-1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0; 
                    }
                }
                else
                {
                    waveTimer = 0; 
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }

            if (waveTimer <= 0 && _spawnedBoats.Count <= 0)
            {
                currentWave++;
                GenerateWave();
            }
        }

        private void GenerateWave()
        {
            WaveValue = currentWave * 10;
            GenerateBoats();
            spawnInterval = waveDuration / boatsToSpawn.Count;
            waveTimer = waveDuration;
        }

        private void GenerateBoats()
        {
            List<GameObject> generatedBoats = new List<GameObject>();
            while (WaveValue > 0 || generatedBoats.Count < 50) 
            {
                int rndBoatId = Random.Range(0, _boats.Count);
                int rndBoatCost = _boats[rndBoatId].spawnCost;

                if (WaveValue - rndBoatCost >= 0) {
                    generatedBoats.Add(_boats[rndBoatId].BoatPrefab);
                    WaveValue -= rndBoatCost;
                }
                else if (WaveValue <= 0) {
                    break;
                }
            }
            boatsToSpawn.Clear();
            boatsToSpawn = generatedBoats;
        }*/
   
    }
}