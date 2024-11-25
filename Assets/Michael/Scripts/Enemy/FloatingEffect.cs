using UnityEngine;

namespace Michael.Scripts.Enemy
{
    public class FloatingEffect : MonoBehaviour
    {
    
        [SerializeField] private Transform _shipModel;
   
    
        [SerializeField] private  AnimationCurve _floatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private  float _floatAmplitude = 0.5f;
        [SerializeField] private  float _floatDuration = 2f;

  
        [SerializeField] private AnimationCurve tiltCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private  float _tiltAmplitude = 5f;
        [SerializeField] private  float _tiltDuration = 2f;

        private float _floatTimer = 0f;
        private float _tiltTimer = 0f;

        void Update()
        {
            if (!_shipModel)
                return;
        
            _floatTimer += Time.deltaTime / _floatDuration;
            _tiltTimer += Time.deltaTime / _tiltDuration;
        
            float floatValue = _floatCurve.Evaluate(_floatTimer % 1) * _floatAmplitude;
            float tiltValue = tiltCurve.Evaluate(_tiltTimer % 1) * _tiltAmplitude;
        
            _shipModel.localPosition = new Vector3(0, floatValue, 0);
            _shipModel.localRotation = Quaternion.Euler(0, 0, tiltValue);
        }
    }
}