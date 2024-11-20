using UnityEngine;
using UnityEngine.AI;

namespace Alexandre
{
    public class BoatController : MonoBehaviour
    {
        [SerializeField] private float _boatSpeed;
        private GameObject _playerTarget;
        private NavMeshAgent _navMeshAgent;
        private bool _HasThief = false;
        private Vector3 _fleeDestination;


        void Start()
        {
            //_playerTarget = GameObject.FindGameObjectWithTag("Player");
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _boatSpeed;
        }


        void Update()
        {
            if (!_HasThief)
            {
                FollowTarget(Vector3.zero);
            }
            else
            {
                FollowTarget(_fleeDestination);
            }
        }

        private void FollowTarget(Vector3 destination)
        {
            _navMeshAgent.SetDestination(destination);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                Destroy(gameObject);
                Debug.Log("bateau touché");
            }

            if (other.CompareTag("Player"))
            {
                Debug.Log("phare atteint");
                _HasThief = true;
                _fleeDestination = Vector3.zero - transform.position;
            }
        }
    }
}