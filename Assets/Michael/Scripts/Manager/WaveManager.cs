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
        [SerializeField] private List<WaveData> _waveData;
        private WaveData _currentWaveData;
        private int _spawnIndex;
        private Transform _lastSpawnPoint;
     
        
        
        
        private void Start() {
            StartWave();
        }

        private void FixedUpdate()
        {
            if (_spawnTimer <= 0) {
                if (_boatsToSpawn.Count > 0) {
                    
                    _spawnIndex = Random.Range(0, _spawnPoints.Count);
                   if(_lastSpawnPoint == _spawnPoints[_spawnIndex]) {
                        
                        _spawnIndex = Random.Range(0, _spawnPoints.Count);
                    }
                    _lastSpawnPoint = _spawnPoints[_spawnIndex];
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

            if ( _spawnedBoats.Count <= 0 && _boatsToSpawn.Count <= 0) { 
                
               EndWave();
            }
            
        }

        [ContextMenu("StartWave !")]
        public void StartWave() {
            
            _currentWaveData = _waveData[_currentWave]; 
            _waveValue += _waveValueIncrement;
            _spawnTimer = 0;
            Debug.Log("Wave Started!");
            // ui alerte 
            // un tuto avant  ?
            _currentWaveValue = _waveValue;
            _currentWave++;
            _waveText.text = "Vague : " + _currentWave;
            
            if (_currentWave % progressionInterval == 0) {
              //  ApplyProgression(); // Augmente les stats toutes les x vagues
                Debug.Log("upgrade ENEMY !!!!");
            }
            GenerateEnemy();
            
        }

        private void GenerateEnemy() {

            if (_currentWave > 10 ) {
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
            else
            {
                foreach (BoatType boat in _currentWaveData.Boats)
                {
                    _boatsToSpawn.Add(boat.BoatPrefab);
                    boat.BoatPrefab.GetComponent<Enemy.BoatEnemy>().BoatType = boat ;
                }
                
            }
          

        }
        private void EndWave() {
           
            GameManager.Instance.OpenShop();
        }
        
       
       /* private void ApplyProgression()
        {
            foreach (BoatType boat in _boat) {
              
                boat.Speed += speedIncrement; 
                boat.GoldCapacity += goldIncrement;
            }
            
        }*/

        
        
        

    }
}
