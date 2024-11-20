using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private List<Transform> _spawnPoints;

    private void Start() {
      InvokeRepeating(nameof(SpwanBoat),0f,5f);
    }

    private void SpwanBoat() {
        int num = Random.Range(0, _spawnPoints.Count);
        Instantiate(_boatPrefab, _spawnPoints[num].position, Quaternion.identity);
    }
    
    
    
   
    
          
 
           
    
    
}
