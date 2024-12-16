using UnityEngine;

namespace Michael.Scripts.Enemy
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f; 
        [SerializeField] private GameObject _slashParticle;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 offset;
        private float startTime;
      
        
    
        private bool _tracjetorySetted = false;

        void Start()
        {
            startTime = Time.time;
        }

        void Update()
        {
            if (!_tracjetorySetted) return;
            float time = (Time.time - startTime) * speed;
            transform.position = GetBezierPoints(startPosition, endPosition, offset, time);
            
        }

        public void SetTrajectoryParameters(Vector3 start, Vector3 end, Vector3 newOffset)
        {
            startPosition = start;
            endPosition = end;
            offset = newOffset;
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
            if (other.CompareTag("Water"))
            {
                Debug.Log("water touched" );
                Instantiate(_slashParticle, new Vector3(transform.position.x,0.1f,transform.position.z), Quaternion.identity);
                SoundManager.PlaySound(SoundType.WaterHit);
                Destroy(gameObject,0.5f);
            }
            else if (other)
            {
                Destroy(gameObject);
            }
           
        }
        
        
    }
}
