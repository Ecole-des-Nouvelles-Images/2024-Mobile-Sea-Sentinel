using UnityEngine;

namespace Michael.Scripts.Manager
{
    [CreateAssetMenu(fileName = "BoatType",menuName = "ScriptableObjects/BoatType")]
    public class BoatType : ScriptableObject
    {
        public GameObject BoatPrefab;
        public int SpawnCost;
        public float Speed;
        public int GoldCapacity;
        public int MaxHealth;
    
    }
}
