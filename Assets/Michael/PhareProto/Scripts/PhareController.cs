using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Alexandre
{
    public class PhareController : MonoBehaviour
    {
        [Header("Phare Settings")]
        public GameObject PhareLight;
        public GameObject CanonOut;
        public GameObject BulletPrefab;
        public Slider inclinationSlider;
        public GameObject RotationController;
        public bool options2 = false;

        [Header("Canon Settings")]
        public float SpeedRotation = 15.0f; // Vitesse de rotation du phare
        public float ElevationSpeed = 100f; // Vitesse d'élévation
        public float LoweringSpeed = 100f; // Vitesse de descente
        public float LoweringDelay = 0.5f; // Délai avant de redescendre
        public float MaxElevationAngle = 45f; // Angle maximum
        public float MinElevationAngle = -45f; // Angle minimum

        private float _elevationAngle = 0f; // État actuel de l'élévation
        private float _rotationAngle = 0f;
        private bool _isElevating = false; // Indique si le canon monte
        private bool _isLowering = false; // Indique si le canon descend

        public Button ShootButton;
        public float BulletSpeed = 10f;
        private bool _isChargingShot;
        private bool _isShooting;
        private float _chargeTime;
        private float _chargePower;

        [Header("UI Elements")]
        public Slider ChargeSlider; // Slider for charge feedback

        public SteeringWheel RotationWheel;
        private bool _isRotatingBack;
        private bool _isRotatingForward;
        private void Start()
        {
            inclinationSlider.onValueChanged.AddListener(OnAimSliderValueChanged);
            //RotationController.onValueChanged.AddListener(OnRotationSliderValueChanged);
            _isChargingShot = false;
            ChargeSlider.value = 0f; // Initialize charge slider value
        }

        public void ChargeShot()
        {
            _isChargingShot = true;
            _chargeTime = 0f; // Reset charge time
        }

        public void ReleaseShot()
        {
            _isShooting = true;
            ChargeSlider.value = 0f; // Reset charge slider value
        }

        private void OnAimSliderValueChanged(float value)
        {
            _elevationAngle = value;
            PhareLight.transform.localEulerAngles = new Vector3(-_elevationAngle, PhareLight.transform.localEulerAngles.y, PhareLight.transform.localEulerAngles.z);
        }

        private void OnRotationSliderValueChanged(float value)
        {
            _rotationAngle = value * 10f;
            PhareLight.transform.rotation = Quaternion.Euler(0, _rotationAngle, 0);
        }
        
        public void RotateWheel(float value)
        {
            if (value < 0)
            {
                _isRotatingBack = true;
                _isRotatingForward = false;
            }
            else if (value > 0)
            {
                _isRotatingBack = false;
                _isRotatingForward = true;
            }
            else
            {
                _isRotatingBack = false;
                _isRotatingForward = false;
            }
 }
         
        

        public void ToggleRotation()
        {
            options2 = !options2;
        }

        private void Update()
        {

            if (options2)
            {
                PhareLight.transform.Rotate(Vector3.up, SpeedRotation * Time.deltaTime, Space.World);
                RotationController.gameObject.SetActive(false);
            }
            else
            {
                RotationController.gameObject.SetActive(true);
            }

            if (_isChargingShot)
            {
                _chargeTime += Time.deltaTime;
                ChargeSlider.value = _chargeTime; // Update charge slider value
                _isLowering = false;
                _isElevating = true;
                if (_isShooting)
                {
                    Shoot();
                    _isElevating = false;
                    _isShooting = false;
                    _isChargingShot = false;
                    StartCoroutine(StartLoweringAfterDelay(LoweringDelay));
                }
            }

            if (_isElevating)
            {
                AdjustElevation(1f, ElevationSpeed);
            }
            else if (_isLowering)
            {
                AdjustElevation(-1f, LoweringSpeed);
                ChargeSlider.value -= 1; // Update charge slider value

            }

            if (_isRotatingBack)
            {
                AdjustRotation(1f);
            }
            else if (_isRotatingForward)
            {
                AdjustRotation(-1f);
            }
        }

        private IEnumerator StartLoweringAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _isLowering = true;
        }
        public void Shoot()
        {
            GameObject bullet = Instantiate(BulletPrefab, CanonOut.transform.position, CanonOut.transform.rotation);
        }

        public void StartElevating()
        {
            _isElevating = true;
        }

        public void StopElevating()
        {
            _isElevating = false;
        }

        public void StartLowering()
        {
            _isLowering = true;
        }

        public void StopLowering()
        {
            _isLowering = false;
        }
        
        private void AdjustRotation(float direction)
        {
            _rotationAngle += direction * SpeedRotation * Time.deltaTime;
            PhareLight.transform.rotation = Quaternion.Euler(0, _rotationAngle, 0);
        }

        private void AdjustElevation(float direction, float speed)
        {
            _elevationAngle += direction * speed * Time.deltaTime;
            _elevationAngle = Mathf.Clamp(_elevationAngle, MinElevationAngle, MaxElevationAngle);
            PhareLight.transform.localEulerAngles = new Vector3(-_elevationAngle, PhareLight.transform.localEulerAngles.y, PhareLight.transform.localEulerAngles.z);
        }
    }
}