using System.Collections.Generic;
using Michael.Scripts.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Manager
{
    public class WaveManager : MonoBehaviourSingleton<WaveManager>
    {
        [Header("Enemy Progression")]
        public int speedIncrement = 2; // Vitesse supplémentaire
        public int goldIncrement = 25; // Capacité d'or supplémentaire
        public int progressionInterval = 3; // Toutes les x vagues

        [Header("Wave Progression")]
        public List<GameObject> _spawnedBoats = new List<GameObject>();
        public List<Transform> _spawnPoints;
        [SerializeField] private int _currentWaveValue = 10;
        [SerializeField] private int _waveValue;
        [SerializeField] private int _currentWave = 0;
        [SerializeField] private int _waveValueIncrement;
        [SerializeField] private List<BoatType> _boat = new List<BoatType>();
        [SerializeField] private List<GameObject> _boatsToSpawn = new List<GameObject>();
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private float _spawnTimer;
        [SerializeField] private TextMeshProUGUI _waveText;
        private int _spawnIndex;
        private Transform _lastSpawnPoint;
     
        
        
        
        private void Start()
        {
            StartWave();
        }

        private void FixedUpdate()
        {
            if (_spawnTimer <= 0) {
                if (_boatsToSpawn.Count > 0) {
                    
                    _spawnIndex = Random.Range(0, _spawnPoints.Count);
                    while (_lastSpawnPoint != _spawnPoints[_spawnIndex]) {
                        
                        _lastSpawnPoint = _spawnPoints[_spawnIndex];
                    }
                    GameObject boat = Instantiate(_boatsToSpawn[0],_lastSpawnPoint.position, Quaternion.identity);
              
                    _boatsToSpawn.RemoveAt(0);
                    _spawnedBoats.Add(boat);
                   _spawnTimer = _spawnInterval;

                    if (_spawnIndex + 1 <= _spawnPoints.Count-1) {
                        _spawnIndex++;
                    }
                    else {
                        _spawnIndex = 0; 
                    }
                }
            }
            else {
                _spawnTimer -= Time.fixedDeltaTime;
            }

            if ( _spawnedBoats.Count <= 0) { 
               _waveValue += _waveValueIncrement;
               StartWave();
            }
            
        }

        [ContextMenu("StartWave !")]
        private void StartWave() {
            _spawnTimer = 0;
            Debug.Log("Wave Started!");
            // ui alerte 
            // un tuto avant  ?
            _currentWaveValue = _waveValue;
            _currentWave++;
            _waveText.text = "Vague : " + _currentWave;
            
            if (_currentWave % progressionInterval == 0) {
                ApplyProgression(); // Augmente les stats toutes les x vagues
                Debug.Log("upgrade ENEMY !!!!");
            }
            GenerateEnemy();
            
        }

        private void GenerateEnemy() {
            while (_currentWaveValue > 0) {
                
                int rndBoat = Random.Range(0, _boat.Count);
                int rndBoatCost = _boat[rndBoat].SpawnCost;
                
                if (_currentWaveValue - rndBoatCost >= 0) {
                    
                    //SpawnBoat(rndBoat);
                    // spawn instantiate 
                     _boatsToSpawn.Add( _boat[rndBoat].BoatPrefab);
                    _currentWaveValue -= rndBoatCost;
                    _boat[rndBoat].BoatPrefab.GetComponent<Enemy.BoatEnemy>().BoatType = _boat[rndBoat];
                }
                else if (_currentWaveValue <= 0) {
                    break;
                }

            }

        }
        private void EndWave() {
            Debug.Log("Wave Completed!");
            // ouvrir le shop 
        }

       /* private void SpawnBoat(int rndBoat)
        {
          //  _spawnIndex = Random.Range(0, _spawnPoints.Count);
            while (_lastSpawnPoint != _spawnPoints[_spawnIndex])
            {
                _spawnIndex = Random.Range(0, _spawnPoints.Count);
                _lastSpawnPoint = _spawnPoints[_spawnIndex];
            }
           
            GameObject boat = Instantiate(_boat[rndBoat].BoatPrefab, _spawnPoints[_spawnIndex].position, Quaternion.identity);
            boat.GetComponent<Enemy.BoatEnemy>().BoatType = _boat[rndBoat];
            Debug.Log(_boat[rndBoat].BoatPrefab);
        }*/
        
       
        private void ApplyProgression()
        {
            foreach (BoatType boat in _boat) {
              
                boat.Speed += speedIncrement; 
                boat.GoldCapacity += goldIncrement;
            }
            
        }

        
        
        

    }
}
