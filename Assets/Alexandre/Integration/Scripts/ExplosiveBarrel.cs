using System.Collections;
using Michael.Scripts.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Alexandre.Integration.Scripts
{
    public class ExplosiveBarrel : MonoBehaviour
    {
        public float ExplosionRadius = 50f;
        public float ExplosionDuration = 1f;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _offset;
        private float _startTime;
        [FormerlySerializedAs("explosionEffect")] public GameObject ExplosionEffectPrefab;
        [FormerlySerializedAs("speed")] public float Speed = .5f; // Vitesse du jet de tonneau

        private bool _tracjetorySetted = false;
        private bool _isExploding = false;

        void Start()
        {
            _startTime = Time.time;
        }

        void Update()
        {
            if (!_tracjetorySetted) return;
            float time = (Time.time - _startTime) * Speed;
            transform.position = GetBezierPoints(_startPosition, _endPosition, _offset, time);
        }

        public void SetTrajectoryParameters(Vector3 start, Vector3 end, Vector3 newOffset)
        {
            _startPosition = start;
            _endPosition = end;
            _offset = newOffset;
            _tracjetorySetted = true;
        }

        private Vector3 GetBezierPoints(Vector3 start, Vector3 end, Vector3 offset, float time)
        {
            Vector3 startToOffset = Vector3.Lerp(start, offset, time);
            Vector3 offsetToEnd = Vector3.Lerp(offset, end, time);
            return Vector3.Lerp(startToOffset, offsetToEnd, time);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Bullet"))
            {
                // Start the explosion coroutine
                StartCoroutine(Explode());
            }
        }

        private IEnumerator Explode()
        {
            if (_isExploding) yield break;
            _isExploding = true;

            Instantiate(ExplosionEffectPrefab, transform.position, Quaternion.identity);
            float maxRadius = ExplosionRadius;
            float duration = ExplosionDuration; // Duration of the explosion expansion
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float currentRadius = Mathf.Lerp(0, maxRadius, elapsedTime / duration);
                Collider[] colliders = Physics.OverlapSphere(transform.position, currentRadius);
                foreach (var hit in colliders)
                {
                    if (hit.CompareTag("Enemy"))
                    {
                        hit.GetComponent<BoatEnemy>().TakeDamage(100);
                    }
                    else if (hit.CompareTag("ExplosiveBarrel"))
                    {
                        ExplosiveBarrel otherBarrel = hit.GetComponent<ExplosiveBarrel>();
                        if (otherBarrel != null && !otherBarrel._isExploding)
                        {
                            otherBarrel.TriggerExplosion();
                        }
                    }
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Destroy the barrel after explosion
            Destroy(gameObject);
        }

        public void TriggerExplosion()
        {
            StartCoroutine(Explode());
        }
    }
}
