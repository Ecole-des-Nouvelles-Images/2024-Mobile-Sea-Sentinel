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
        private int _currentHeath;
        
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
            _currentHeath = _maxHealth;
            
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
                Debug.Log("bateau touché");
            }

            if (other.CompareTag("Player")) {
                Debug.Log("phare atteint" ); 
                _hasThief = true;
                FollowTarget(_initialPosition);
               // PhareController.Goldnumber -= _boatGold;
            }
        }


        private void stealGold()
        {
      
        }
    
    }
}
