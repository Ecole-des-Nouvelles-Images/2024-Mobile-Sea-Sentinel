using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BoatController : MonoBehaviour
{
    [SerializeField] private float _boatSpeed;
    private GameObject _playerTarget;
    private NavMeshAgent _navMeshAgent;
    public bool _HasThief = false;
    //public SpawnManager spawnManager;
   // private Vector3 fleeDestination;
   public Vector3 initialposition;
  
    void Start()
    {
       // spawnManager = FindObjectOfType<SpawnManager>();
        _playerTarget = GameObject.FindGameObjectWithTag("Player");
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _boatSpeed;
        initialposition = transform.position;

     //  fleeDestination = spawnManager._spawnPoints[Random.Range(0, spawnManager._spawnPoints.Count)].position;
    }

    
    void Update()
    {
        if (!_HasThief)
        {
            FollowTarget(_playerTarget.transform.position);

        }
        else
        {  
            //FollowTarget(fleeDestination);
            FollowTarget(initialposition);
            if (_navMeshAgent.remainingDistance < 1)
            {
                Destroy(gameObject);
            }

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
            Debug.Log("phare atteint" );
            _HasThief = true;
            
           
        }
    }
}
