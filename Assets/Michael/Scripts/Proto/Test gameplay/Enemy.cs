using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 100; // PV de base
    public int goldValue = 25; // Or volé ou récompensé
    public float speed = 5f; // Vitesse de déplacement
    private WaveManager waveManager;

    void Start()
    {
        waveManager = FindObjectOfType<WaveManager>();
    }

    void Update()
    {
        MoveTowardsIsland();
    }

    void MoveTowardsIsland()
    {
        // Déplacement simple vers l’île (à adapter selon ton jeu)
        Vector3 target = Vector3.zero; // Position de l’île
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Si atteint l'île
        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            StealGold();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        waveManager.EnemyDefeated();
        Destroy(gameObject);
    }

    void StealGold()
    {
        // Signal au GameManager que de l'or est volé (à implémenter)
        Debug.Log("Gold stolen!");
        waveManager.EnemyDefeated();
        Destroy(gameObject);
    }
}
