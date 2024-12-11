using System.Collections.Generic;
using Michael.Scripts.Controller;
using Michael.Scripts.Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Michael.Scripts.Manager
{
    public class WaveManager : MonoBehaviourSingleton<WaveManager>
    {
        [Header("Enemy Progression")]
        public int SpeedIncrement = 1; 
        public int GoldIncrement = 25; 
        public int HealthIncrement = 10;
        public float BoatGoldMultiplier = 0.2f;  
        public int StatsprogressionInterval = 3; 
        public int WaveprogressionInterval = 5;
        public int _upgradeNumber = 0; 
        
        [Header("Wave Progression")]
        public List<GameObject> _spawnedBoats = new List<GameObject>();
        public List<Transform> _spawnPoints; 
        public WaveData _currentWaveData;
        [SerializeField]private int _littleBoatsIncrement  ;
        [SerializeField]private int _balancedBoatsIncrement ;
        [SerializeField] private int _bigBoatsIncrement = 0 ;
        [SerializeField] private List<BoatType> _boat = new List<BoatType>();
        [SerializeField] private List<GameObject> _boatsToSpawn = new List<GameObject>();
        [SerializeField] private float _spawnInterval = 5f;
        [SerializeField] private float _spawnTimer;
        [SerializeField] private TextMeshProUGUI _waveText;
        [SerializeField] private List<WaveData> _waveData;
        private int _boatsWithGoldGenerated = 0; // Nombre de bateaux riches générés
        private int _spawnIndex;
        private Transform _lastSpawnPoint;
        private int _littleBoatsBonus = 0 ;
        private int _balancedBoatsBonus = 0 ;
        private int _bigBoatsBonus = 0 ;
        private int _currentWave = 0;
        
        
        private void Start() {
            _currentWaveData = _waveData[_currentWave];
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
                    BoatEnemy boatEnemy = boat.GetComponent<Enemy.BoatEnemy>();
                    
                    boatEnemy.InitializeBoatStats();
                    float currentGoldBoatRatio = (float)_boatsWithGoldGenerated / _boatsToSpawn.Count;
                    if (currentGoldBoatRatio < _currentWaveData.BoatWithGoldPourcent)
                    {
                        Debug.Log("boat with golds");
                        int goldOnBoat = Mathf.CeilToInt(boat.GetComponent<BoatEnemy>().BoatGoldMax * 0.25f);
                        boat.GetComponent<Enemy.BoatEnemy>().SetGoldOnBoat(goldOnBoat);
                        _boatsWithGoldGenerated++;
                    }
                    
                    
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
                
               //Invoke(nameof(EndWave),1f);
               EndWave();
            }
        }
        
        [ContextMenu("StartWave !")]
        public void StartWave() {
            
            
            if (_currentWave > (_waveData.Count-1) * WaveprogressionInterval) {
                
                _littleBoatsBonus += _littleBoatsIncrement;
                _balancedBoatsBonus += _balancedBoatsIncrement;
                _bigBoatsBonus += _bigBoatsIncrement;
            }
            _spawnTimer = 0;
           
            // ui alerte 
            _currentWave++;
            _waveText.text = "Vague : " + _currentWave;
            
             if (_currentWave % WaveprogressionInterval == 0 ) {
                
                _currentWaveData = _waveData[_currentWave / WaveprogressionInterval];
                // nouvelle vague 
             }
           /*  if (_currentWave % StatsprogressionInterval == 0)
             {
                 _upgradeNumber++;
                 Debug.Log("stats wave !");
             }*/
           
            if (_currentWave > 1)
            {
                _upgradeNumber++;
            } 
            GenerateEnemy();
            
        }

        private void GenerateEnemy() {

                for (int i = 0; i < _currentWaveData.LittleBoatCount + _littleBoatsBonus; i++)
                {
                    _boatsToSpawn.Add(_boat[0].BoatPrefab);
                }
                for (int i = 0; i < _currentWaveData.BalancedBoatCount + _balancedBoatsBonus; i++)
                {
                    _boatsToSpawn.Add(_boat[1].BoatPrefab);
                }
                for (int i = 0; i < _currentWaveData.BigBoatCount + _bigBoatsBonus; i++)
                {
                    _boatsToSpawn.Add(_boat[2].BoatPrefab);
                }

        }
        private void EndWave() {
            
            _boatsWithGoldGenerated = 0;
            GameManager.Instance.OpenShop();
        }
        
       
        
       
    }
}
