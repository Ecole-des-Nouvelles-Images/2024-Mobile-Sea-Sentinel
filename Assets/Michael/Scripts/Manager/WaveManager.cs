using System.Collections.Generic;
using Michael.Scripts.Controller;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Manager
{
    public class WaveManager : MonoBehaviour
    {
        public List<Transform> _spawnPoints;
        [SerializeField] private int _currentWaveValue = 10;
        [SerializeField] private int _waveValue;
        [SerializeField] private int _currentWave = 0;
        [SerializeField] private List<BoatType> _boat = new List<BoatType>();
        [SerializeField] private List<GameObject> _boatsToSpawn = new List<GameObject>();
        [SerializeField] private List<GameObject> _spawnedBoats = new List<GameObject>();
        private Transform _lastSpawnPoint;
        private void Start()
        {

            StartWave();
        }

        private void Update()
        {

        }

        [ContextMenu("StartWave !")]
        private void StartWave()
        {

            Debug.Log("Wave Started!");

            _waveValue += 5;
            _currentWaveValue = _waveValue;
            _currentWave++;

            GenerateEnemy();
        }

        private void GenerateEnemy()
        {
            while (_currentWaveValue > 0)
            {

                int rndBoat = Random.Range(0, _boat.Count);
                int rndBoatCost = _boat[rndBoat].SpawnCost;


                if (_currentWaveValue - rndBoatCost >= 0)
                {
                    SpawnBoat(rndBoat);
                    // spawn instantiate 
                    // _boatsToSpawn.Add( _boat[rndBoat].BoatPrefab);
                    _currentWaveValue -= rndBoatCost;
                }
                else if (_currentWaveValue <= 0)
                {
                    break;
                }

            }

        }
        private void EndWave()
        {
            Debug.Log("Wave Completed!");
            // ouvrir le shop 
        }

        private void SpawnBoat(int rndBoat)
        {
            int num = Random.Range(0, _spawnPoints.Count);
            while (_lastSpawnPoint != _spawnPoints[num])
            {
                _lastSpawnPoint = _spawnPoints[num];
                GameObject boat = Instantiate(_boat[rndBoat].BoatPrefab, _spawnPoints[num].position, Quaternion.identity);
                boat.GetComponent<Enemy.BoatEnemy>().BoatType = _boat[rndBoat];
                Debug.Log(_boat[rndBoat].BoatPrefab);
            }
            
          
        }

    }
}
