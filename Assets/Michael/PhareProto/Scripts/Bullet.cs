using UnityEngine;

namespace Alexandre
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 30f; // Vitesse de déplacement de la balle
        public float maxHeight = 10f; // Hauteur maximale de la trajectoire en cloche
        public Vector3 targetPosition; // Position cible fournie par l'autre script

        private Vector3 startPosition;
        private Vector3 controlPoint;
        private float journeyLength;
        private float startTime;

        private void Start()
        {
            startPosition = transform.position;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;

            // Calculer le point de contrôle intermédiaire en fonction de la hauteur maximale
            controlPoint = (startPosition + targetPosition) / 2 + Vector3.up * maxHeight;
        }

        private void Update()
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // Calculer la position le long de la courbe de Bézier quadratique
            Vector3 currentPosition = CalculateQuadraticBezierPoint(fractionOfJourney, startPosition, controlPoint, targetPosition);
            transform.position = currentPosition;
            if (fractionOfJourney >= 1)
            {
                Destroy(gameObject);
            }
        }

        private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;
            return p;
        }
    }
}
