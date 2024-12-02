using UnityEngine;

namespace Alexandre
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 30f; // Vitesse de déplacement de la balle
        public float lifeTime = 5f; // Durée de vie de la balle avant destruction

        private void Start()
        {
            // Détruire automatiquement la balle après `lifeTime` secondes
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            // Faire avancer la balle tout droit
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}