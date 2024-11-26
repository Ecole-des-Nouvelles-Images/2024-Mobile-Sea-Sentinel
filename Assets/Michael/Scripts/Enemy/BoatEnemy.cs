using Michael.Scripts.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Michael.Scripts.Enemy
{
  
    public class BoatEnemy : MonoBehaviour
    {
        [Header("Boat Data")]
        public BoatType BoatType;
        private int _boatGoldMax; 
        private int _currentBoatGold = 0;
        private int _maxHealth;
        private int _currentHealth;

        private GameObject _playerTarget;
        private NavMeshAgent _navMeshAgent;
        private bool _hasThief = false;
        private Vector3 _initialPosition;
   
        

        void Start() {
        
            _playerTarget = GameObject.FindGameObjectWithTag("Player");
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _initialPosition = transform.position;
           _navMeshAgent.speed = BoatType.Speed;
            _boatGoldMax = BoatType.GoldCapacity;
            _maxHealth = BoatType.MaxHealth;
            _currentHealth = _maxHealth;
            
            FollowTarget(_playerTarget.transform.position);
        }
        
        

    
        void Update() {
            if (!_hasThief) return;
            if (_navMeshAgent.remainingDistance < 1) {
                Destroy(gameObject);
            }
        }

        private void FollowTarget(Vector3 destination) {
            _navMeshAgent.SetDestination(destination);
        }


        private void OnTriggerEnter(Collider other) {
        
            if (other.CompareTag("Bullet")) {
                Destroy(gameObject);
               // TakeDamage(10);
                Debug.Log("bateau touché");
            }

            if (other.CompareTag("Player")) {
                Debug.Log("phare atteint" ); 
                _hasThief = true;
                FollowTarget(_initialPosition);
               // PhareController.Goldnumber -= _boatGold;
            }
        }
        
        
        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Debug.Log($"{BoatType.BoatPrefab.name} détruit!");
                Destroy(gameObject);
            }
        }

        private void stealGold()
        {
      
        }
    
    }
}
