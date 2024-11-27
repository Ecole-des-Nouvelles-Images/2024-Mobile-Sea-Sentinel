using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Michael.PhareProto.Scripts
{
    public class PhareController : MonoBehaviour
    {
        [FormerlySerializedAs("PhareLight")] [Header("Phare Settings")]
        public GameObject Pharecanon;
        public GameObject PhareRailcanon;
        public GameObject CanonOut;
        public GameObject BulletPrefab;
        public Slider inclinationSlider;
        public Slider RotationSlider;
        public bool options2 = false;
       
        public static int Goldnumber;
        public int MaxGoldCapacity = 100;
        public float FireRate = 1;
        public TextMeshProUGUI goldText;
        
        [Header("Canon Settings")]
        public float SpeedRotation = 15.0f; // Vitesse de rotation du phare
        public float ElevationSpeed = 100f; // Vitesse d'élévation
        public float MaxElevationAngle = 45f; // Angle maximum
        public float MinElevationAngle = -45f; // Angle minimum

        private float _elevationAngle = 0f; // État actuel de l'élévation
        private float _rotationAngle = 0f;
        private bool _isElevating = false; // Indique si le canon monte
        private bool _isLowering = false; // Indique si le canon descend


        
        private void Start()
        {
           
            Goldnumber = MaxGoldCapacity;
            inclinationSlider.onValueChanged.AddListener(OnAimSliderValueChanged);
            RotationSlider.onValueChanged.AddListener(OnRotationSliderValueChanged);
        }

      
        private void OnAimSliderValueChanged(float value)
        {
            _elevationAngle = value;
            Pharecanon.transform.localEulerAngles = new Vector3(-_elevationAngle, Pharecanon.transform.localEulerAngles.y, Pharecanon.transform.localEulerAngles.z);
        }
        
        private void OnRotationSliderValueChanged(float value)
        {
            _rotationAngle = value * 10f;
            PhareRailcanon.transform.localRotation = Quaternion.Euler(0,0 , _rotationAngle);
        }

        public void ToggleRotation()
        {
            options2 = !options2;
        }

        private void Update()
        {
            goldText.text = Goldnumber + " / " + MaxGoldCapacity;
            // Rotation continue du phare
            if (options2)
            {
                Pharecanon.transform.Rotate(Vector3.up, SpeedRotation * Time.deltaTime, Space.World);
                RotationSlider.gameObject.SetActive(false);
                Debug.Log("tourne");
            }
            else
            {
                RotationSlider.gameObject.SetActive(true);
            }
            

           
        

            // Ajustement continu de l'élévation
            if (_isElevating)
            {
                AdjustElevation(1f);
            }
            else if (_isLowering)
            {
                AdjustElevation(-1f);
            }
        }

        // Méthode appelée par le bouton "Tirer"
        public void Shoot()
        {
            Instantiate(BulletPrefab, CanonOut.transform.position, CanonOut.transform.rotation);
        }

        // Méthodes pour élever et abaisser le canon
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

        private void AdjustElevation(float direction)
        {
            _elevationAngle += direction * ElevationSpeed * Time.deltaTime;
            _elevationAngle = Mathf.Clamp(_elevationAngle, MinElevationAngle, MaxElevationAngle);
            Pharecanon.transform.localEulerAngles = new Vector3(-_elevationAngle, Pharecanon.transform.localEulerAngles.y, Pharecanon.transform.localEulerAngles.z);
        }
    }
}


