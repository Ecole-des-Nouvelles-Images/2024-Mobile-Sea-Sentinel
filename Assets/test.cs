using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    public float detectionRadius = 10f; // Distance de détection des autres bateaux
    public float avoidanceStrength = 5f; // Intensité de l'évitement
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Collider[] nearbyBoats = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Boats"));

        Vector3 avoidanceDirection = Vector3.zero;

        foreach (var boat in nearbyBoats)
        {
            if (boat.gameObject != this.gameObject) // Ignorer soi-même
            {
                Vector3 directionToBoat = transform.position - boat.transform.position;
                float distance = directionToBoat.magnitude;
                if (distance > 0)
                {
                    avoidanceDirection += directionToBoat.normalized / distance; // Évitement proportionnel à la distance
                }
            }
        }

        if (avoidanceDirection != Vector3.zero)
        {
            agent.velocity += avoidanceDirection.normalized * avoidanceStrength * Time.deltaTime;
        }
    }
}