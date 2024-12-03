using UnityEngine;

namespace Michael.Scripts.Manager
{
    [CreateAssetMenu(fileName = "BoatType",menuName = "ScriptableObjects/BoatType")]
    public class BoatType : ScriptableObject
    {
        [SerializeField] GameObject _boatPrefab;
        public GameObject BoatPrefab { get => _boatPrefab; private set => _boatPrefab = value; }

        [SerializeField] int _spawnCost;
        public int SpawnCost  { get => _spawnCost; private set => _spawnCost = value; }
        
        [SerializeField] float _speed;
        public float Speed  { get => _speed; private set => _speed = value; }
        
        [SerializeField] int _goldCapacity;
        public int GoldCapacity  { get => _goldCapacity; private set => _goldCapacity = value; }
    
        [SerializeField] int _maxHealth;
        public int MaxHealth  { get => _maxHealth; private set => _maxHealth = value; }
       
    
    }
}
