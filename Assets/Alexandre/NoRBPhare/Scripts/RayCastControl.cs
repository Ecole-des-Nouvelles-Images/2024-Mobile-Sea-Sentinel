using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Alexandre.NoRBPhare.Scripts
{
    public class RayCastControl : MonoBehaviour
    {
        [FormerlySerializedAs("autoShoot")] [SerializeField]
        private bool _autoShoot = false;

        public GameObject RayCastUI;

        // Définissez le masque de couche pour la couche "Interactable"
        [FormerlySerializedAs("interactableLayer")]
        public LayerMask InteractableLayer;

        // Référence à la lumière
        [FormerlySerializedAs("phareLight")] public GameObject PhareLight;

        public float AimSpeed = 5f;

        public Transform CanonRail;
        public GameObject CrosshairPrefab;
        private GameObject _crosshairInstance;

        // Position touchée
        private Vector3 _touchPosition;

        private Vector3 _targetPosition;

        public float ShootCooldown = 2.0f; // Cooldown time in seconds
        private float _lastShootTime = 0f; // Time when the last shot was fired

        // Références pour le tir
        public GameObject BulletPrefab;
        public ParticleSystem VFXShot1;
        public ParticleSystem VFXShot2;
        public Transform CanonOut;

        // Ref au Canon
        public Transform Canon;

        // Reference CoolDown slider
        public Slider CoolDownSlider;

        public float minDistance = 5f; // Distance minimale pour viser
        public float maxDistance = 50f; // Distance maximale pour viser

        public float _depthOffset = 5f;
        public float _heightOffsetDelta = 1f;

        [SerializeField] private Transform _offset;

        void Start()
        {
            RayCastUI.SetActive(true);
            CoolDownSlider.maxValue = ShootCooldown;
            CoolDownSlider.value = 0; // Initialiser à la valeur minimale
            _crosshairInstance = Instantiate(CrosshairPrefab);

            // Ajuster le masque de couche pour exclure le layer "NonInteractable"
            InteractableLayer = ~LayerMask.GetMask("NonInteractable");
        }

        void Update()
        {
            UpdateLightDirection();
            UpdateCoolDownSlider();
            UpdateCanonDirection();

            // Vérifiez s'il y a des touches sur l'écran
            if (Input.touchCount > 0)
            {
                // Obtenez le premier toucher
                Touch touch = Input.GetTouch(0);
                Debug.Log("Touch phase : " + touch.phase);

                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Touch is over a UI element");
                    return;
                }

                // Vérifiez si le toucher est une phase de début (quand le joueur commence à toucher l'écran)
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary)
                {
                    // Convertissez la position du toucher en un rayon
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    Debug.Log("Ray : " + ray);

                    // Effectuez un raycast avec le masque de couche
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer))
                    {
                        // Si le raycast touche quelque chose, obtenez la position
                        _touchPosition = hit.point;
                        Debug.Log("Position touchée : " + _touchPosition);

                        // Calculez la distance en deux dimensions (x, z)
                        Vector3 horizontalOffset = new Vector3(_touchPosition.x, 0, _touchPosition.z) -
                                                   new Vector3(Canon.position.x, 0, Canon.position.z);
                        float distanceToTarget = horizontalOffset.magnitude;
                        distanceToTarget = Mathf.Clamp(distanceToTarget, minDistance, maxDistance);

                        // Ajuster la position touchée en fonction de la distance ajustée tout en conservant la valeur de Y
                        Vector3 adjustedPosition = Canon.position + horizontalOffset.normalized * distanceToTarget;
                        _touchPosition = new Vector3(adjustedPosition.x, _touchPosition.y, adjustedPosition.z);

                        //Ajuster l'Offset
                        CalculateOffset(distanceToTarget);
                        //Ajuste l'inclinaison du Canon
                        UpdateCanonDirection();

                        // Mettez à jour la direction de la lumière
                        UpdateLightDirection();
                        UpdateCrosshairPosition(_touchPosition);
                    }
                }

                if (touch.phase == TouchPhase.Ended && _autoShoot)
                {
                    ShootWithCooldown();
                }
            }
        }


        void CalculateOffset(float distanceToTarget)
        {
            float offsetHeight = Mathf.Lerp(_touchPosition.y, Canon.position.y * _heightOffsetDelta,
                (distanceToTarget - minDistance) / (maxDistance - minDistance));
            Debug.Log("Offset Height : " + offsetHeight);

            _offset.position = new Vector3(_touchPosition.x, offsetHeight, _touchPosition.z);
            Debug.Log("Offset position : " + _offset.position);
        }


        void UpdateLightDirection()
        {
            if (PhareLight != null && _touchPosition != Vector3.zero)
            {
                // Calculez la direction de la lumière vers la position touchée
                Vector3 direction = (_touchPosition - PhareLight.transform.position).normalized;

                // Interpolez la direction de la lumière en utilisant le delta time et la vitesse
                PhareLight.transform.forward =
                    Vector3.Lerp(PhareLight.transform.forward, direction, AimSpeed * Time.deltaTime);
            }
        }

        void UpdateCrosshairPosition(Vector3 hitPoint)
        {
            _crosshairInstance.transform.position = hitPoint;
        }

        void UpdateCanonDirection()
        {
            if (_touchPosition != Vector3.zero)
            {
                // Calculez la direction du canon vers la position touchée
                Vector3 direction = (_offset.position - Canon.position).normalized;

                // Interpolez la direction du canon en utilisant le delta time et la vitesse
                Canon.forward = Vector3.Lerp(Canon.forward, direction, AimSpeed * Time.deltaTime);

                Vector3 lookAtPosition =
                    new Vector3(_offset.position.x, CanonRail.transform.position.y, _offset.position.z);
                CanonRail.transform.LookAt(lookAtPosition);
            }
        }

        public void ShootWithCooldown()
        {
            if (Time.time >= _lastShootTime + ShootCooldown)
            {
                // Vérifiez si la position de tir est valide
                if (_touchPosition != Vector3.zero)
                {
                    //float distanceToTarget = Vector3.Distance(Canon.position, touchPosition);

                    // VFX de tir
                    if (VFXShot1 != null) VFXShot1.Play();
                    if (VFXShot2 != null) VFXShot2.Play();
                    // Instancier le boulet
                    GameObject bullet = Instantiate(BulletPrefab, CanonOut.position, CanonOut.rotation);
                    Vector3 bulletEndTarget = _touchPosition + Vector3.up * -_depthOffset;
                    bullet.GetComponent<Bullet>()
                        .SetTrajectoryParameters(CanonOut.position, bulletEndTarget, _offset.position);
                    // Définir la direction du boulet pour correspondre à la direction du Canon
                    bullet.transform.forward = Canon.forward;

                    // Mettre à jour le dernier temps de tir
                    _lastShootTime = Time.time;

                    // Réinitialiser le slider à la valeur minimale
                    CoolDownSlider.value = 0;
                }
            }
        }

        void UpdateCoolDownSlider()
        {
            float timeSinceLastShot = Time.time - _lastShootTime;
            CoolDownSlider.value = Mathf.Clamp(timeSinceLastShot, 0, ShootCooldown);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_offset.position, 2f);
        }
    }
}