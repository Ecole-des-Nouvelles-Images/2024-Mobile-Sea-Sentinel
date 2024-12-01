using UnityEngine;

namespace ProtoPharePackage
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 30f; // Vitesse de déplacement de la balle
        public float lifeTime = 5f; // Durée de vie de la balle avant destruction
        public float controlHeight = 10f; // Hauteur du point de contrôle

        private float elapsedTime = 0f;
        private Vector3 startPosition;
        private Vector3 target;
        private bool isInitialized = false;
        private void Start()
        {
            // Détruire automatiquement la balle après `lifeTime` secondes
            Destroy(gameObject, lifeTime);
            startPosition = transform.position;
        }

        private void Update()
        {
            if (isInitialized = false) return;
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lifeTime;

            // Calculer la position du point de contrôle
            Vector3 controlPoint = new Vector3((startPosition.x + target.x) / 2, controlHeight, (startPosition.z + target.z) / 2);

            // Calculer la position de la balle le long de la courbe de Bézier quadratique
            Vector3 bezierPosition = Mathf.Pow(1 - t, 2) * startPosition +
                                     2 * (1 - t) * t * controlPoint +
                                     Mathf.Pow(t, 2) * target;

            // Mettre à jour la position de la balle
            transform.position = bezierPosition;
        }

        public void Initialize(Vector3 targetPosition)
        {
            target = targetPosition;
            isInitialized = true;
        }
    }
}