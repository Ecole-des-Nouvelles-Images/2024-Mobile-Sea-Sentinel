using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alexandre
{
    public class SentinelController : MonoBehaviour
    {
        [Header("Sentinel Elements")]
        public GameObject SentinelLight;
        public GameObject CanonOut;
        public GameObject BulletPrefab;

        [Header("UI Elements")]
        public Slider ChargeSlider; // Slider for charge feedback
        public GameObject RotationWheel;

        [Header("Sentinel Settings")]
        public float SpeedRotation = 15.0f; // Vitesse de rotation du phare
        public float ElevationSpeed = 100f; // Vitesse d'élévation
        public float LoweringSpeed = 100f; // Vitesse de descente
        public float LoweringDelay = 0.5f; // Délai avant de redescendre
        public float MaxElevationAngle = 45f; // Angle maximum
        public float MinElevationAngle = -45f; // Angle minimum

        private bool _isChargingShot = false;
        private bool _isReturning = false;
        private float _chargeTime = 0f;
        private float _returningTime = 0f;

        public void ChargeShot()
        {
            _isReturning = false;
            _isChargingShot = true;
            _chargeTime = 0f; // Reset charge time
        }

        public void ReleaseShot()
        {
            _returningTime = 0f;
            _isChargingShot = false;
            ChargeSlider.value = 0f;
            StartCoroutine(ReturnToMinimumAngle());
        }

        private IEnumerator ReturnToMinimumAngle()
        {
            yield return new WaitForSeconds(LoweringDelay);
            _isReturning = true;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_isChargingShot)
            {
                _chargeTime += Time.deltaTime;
                ChargeSlider.value = _chargeTime;
                float tiltAngle = Mathf.Clamp(_chargeTime * ElevationSpeed, MinElevationAngle, MaxElevationAngle);
                SentinelLight.transform.localRotation = Quaternion.Euler(-tiltAngle, 0, 0);
            }

            if (_isReturning)
            {
                Quaternion targetRotation = Quaternion.Euler(MinElevationAngle, 0, 0);
                SentinelLight.transform.localRotation = Quaternion.RotateTowards(SentinelLight.transform.localRotation, targetRotation, LoweringSpeed * Time.deltaTime);

                if (SentinelLight.transform.localRotation == targetRotation)
                {
                    _isReturning = false;
                }
            }
        }
    }
}
