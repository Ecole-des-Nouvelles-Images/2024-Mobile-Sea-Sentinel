using System;
using System.Collections;
using System.Collections.Generic;
using Michael.PhareProto.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BoatController : MonoBehaviour
{
    public int boatGold;
    public int boatGoldMax;
    [SerializeField] private float _boatSpeed;
    private GameObject _playerTarget;
    private NavMeshAgent _navMeshAgent;
    private bool _hasThief = false;
    //public SpawnManager spawnManager;
    // private Vector3 fleeDestination;
    public Vector3 initialposition;
    public static Action OnGoldRobbed;

    private void OnEnable()
    {
        OnGoldRobbed += stealGold;
    }

    private void OnDisable()
    {
        OnGoldRobbed -= stealGold;
    }

    void Start()
    {
        transform.rotation = Quaternion.identity;  
       // spawnManager = FindObjectOfType<SpawnManager>();
        _playerTarget = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _boatSpeed;
        initialposition = transform.position;
        FollowTarget(_playerTarget.transform.position);
     //  fleeDestination = spawnManager._spawnPoints[Random.Range(0, spawnManager._spawnPoints.Count)].position;
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
            FollowTarget(initialposition);
           PhareController.Goldnumber -= boatGold;
        }
    }


    private void stealGold()
    {
      
    }
    
}
