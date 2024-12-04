using UnityEngine;

namespace Alexandre
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 30f; // Vitesse de déplacement de la balle
        public Vector3 targetPosition; // Position cible fournie par l'autre script

        private Vector3 startPosition;
        private Vector3 controlPoint;
        private float journeyLength;
        private float startTime;
        private bool initialized = false;

        public void InitializeTrajectory(float minHeight, float maxHeight, float maxDistance)
        {
            startPosition = transform.position;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;

            // Calculer la hauteur maximale en fonction de la distance
            float distance = Vector3.Distance(startPosition, targetPosition);
            float height = minHeight + (maxHeight - minHeight) * Mathf.Pow(distance / maxDistance, 2);
            controlPoint = (startPosition + targetPosition) / 2 + Vector3.up * height;

            initialized = true;
        }

        private void Update()
        {
            if (!initialized)
            {
                return;
            }
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
