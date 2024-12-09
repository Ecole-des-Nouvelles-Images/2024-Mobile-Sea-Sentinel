using UnityEngine;
using UnityEngine.Serialization;

namespace Alexandre.NoRBPhare.Scripts
{
    public class ExplosiveBarrel : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private Vector3 _offset;
        private float _startTime;
        [FormerlySerializedAs("speed")] public float Speed = 10f; // Vitesse du jet de tonneau
    
        private bool _tracjetorySetted = false;

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
            if (other.gameObject.CompareTag("Enemy"))
            {
                //blast with a sphere collider to apply damage to enemies
                //destroy the barrel
            }
            // Détruire le projectile lorsqu'il entre en collision avec un autre objet
            Destroy(gameObject);
        }
    }
}