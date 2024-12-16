using System;
using Michael.Scripts.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Intégration.Scripts
{
    public class RayCastControl : MonoBehaviour
    {
         [FormerlySerializedAs("autoShoot")] [SerializeField]
        private bool _autoShoot = false;

      //  public GameObject RayCastUI;

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

        // Arme sélectionnée
        public enum WeaponType
        {
            Canon = 0,
            ExplosiveBarrel = 1
        }

        public WeaponType SelectedWeapon;
        
        // Références pour le tir
        public GameObject BulletPrefab;
        public ParticleSystem VFXShot1;
        public ParticleSystem VFXShot2;
        public Transform CanonOut;
        [SerializeField] private Transform _offset;

        public float _depthOffset = 5f;
        public float _heightOffsetDelta = 1f;
        
        // Références pour le tir de tonneaux explosifs
       
        public Transform BarrelOut;
        public GameObject ExplosiveBarrelPrefab;
        public Transform BarrelOffset;
        [SerializeField] private float _barrelHeightOffsetDelta = 5f;
        

        // Ref au Canon
        public Transform Canon;

        // Cooldown radial
        public Image CoolDownImage;
        // Reference CoolDown slider
        //public Slider CoolDownSlider;

        public float minDistance = 5f; // Distance minimale pour viser
        public float maxDistance = 50f; // Distance maximale pour viser
        
        // Ref au playerData ( pour les explosive barrels ) 
        
        // Référence à la caméra principale
        public Camera MainCamera;



        void Start()
        {
            
            CoolDownImage.fillAmount = 0;
            //RayCastUI.SetActive(true);
            //CoolDownSlider.maxValue = ShootCooldown;
            //CoolDownSlider.value = 0; // Initialiser à la valeur minimale
            _crosshairInstance = Instantiate(CrosshairPrefab);
            // Ajuster le masque de couche pour exclure le layer "NonInteractable"
            InteractableLayer = ~LayerMask.GetMask("NonInteractable");

            // Initialiser SelectedWeapon à la première arme
            SelectedWeapon = WeaponType.Canon;
        }

        void Update()
        {
         //   UpdateLightDirection();
         //UpdateCoolDownSlider();
         UpdateCoolDownImage();
         UpdateCanonDirection();

            // Vérifiez s'il y a des touches sur l'écran
            if (Input.touchCount > 0)
            {
                // Obtenez le premier toucher
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }

                // Vérifiez si le toucher est une phase de début (quand le joueur commence à toucher l'écran)
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved ||
                    touch.phase == TouchPhase.Stationary)
                {
                    // Convertissez la position du toucher en un rayon
                    Ray ray =  MainCamera.ScreenPointToRay(touch.position);

                    // Effectuez un raycast avec le masque de couche
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, InteractableLayer))
                    {
                        // Si le raycast touche quelque chose, obtenez la position
                        _touchPosition = hit.point;

                        // Calculez la distance en deux dimensions (x, z)
                        Vector3 horizontalOffset = new Vector3(_touchPosition.x, 0, _touchPosition.z) -
                                                   new Vector3(Canon.position.x, 0, Canon.position.z);
                        float distanceToTarget = horizontalOffset.magnitude;
                        distanceToTarget = Mathf.Clamp(distanceToTarget, minDistance, maxDistance);

                        // Ajuster la position touchée en fonction de la distance ajustée tout en conservant la valeur de Y
                        Vector3 adjustedPosition = Canon.position + horizontalOffset.normalized * distanceToTarget;
                        _touchPosition = new Vector3(adjustedPosition.x, _touchPosition.y, adjustedPosition.z);

                        
                        //Ajuster les Offset
                        GetExplosiveBarrelTrajectory();

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
                    CanonShootWithCooldown();
                }
            }
        }

        public void SelectNextWeapon()
        {
            SelectedWeapon = (WeaponType)(((int)SelectedWeapon + 1) % Enum.GetValues(typeof(WeaponType)).Length);
        }
        
        //  public void Shoot()
        // {
        //
        //         switch (SelectedWeapon)
        //         {
        //             case WeaponType.Canon:
        //                 CanonShootWithCooldown();
        //                 break;
        //             case WeaponType.ExplosiveBarrel:
        //                 ShootExplosiveBarrel();
        //                 break;
        //             default:
        //                 CanonShootWithCooldown();
        //                 break;
        //         }
        //     
        //
        // }
        
        public void ShootExplosiveBarrel()
        {
            if (PlayerData.Instance.CurrentExplosifBarrel > 0)
            {
                GameObject barrel = Instantiate(ExplosiveBarrelPrefab, BarrelOut.position, BarrelOut.rotation);
                barrel.GetComponent<ExplosiveBarrel>()
                    .SetTrajectoryParameters(BarrelOut.position, _touchPosition, BarrelOffset.position);
                PlayerData.Instance.CurrentExplosifBarrel--;
                PlayerData.Instance.UpdateExplosiveBarrelText();
            }
            else
            {
                SoundManager.PlaySound(SoundType.ShootFail);
            }
        }


        public void CanonShootWithCooldown()
        {
            if (Time.time >= _lastShootTime + ShootCooldown)
            {
                // Vérifiez si la position de tir est valide
                if (_touchPosition != Vector3.zero)
                {
                    //float distanceToTarget = Vector3.Distance(Canon.position, touchPosition);
                    SoundManager.PlaySound(SoundType.Shoot);
                    // VFX de tir
                    if (VFXShot1 != null) VFXShot1.Play();
                    if (VFXShot2 != null) VFXShot2.Play();
                    // Instancier le boulet
                    GameObject bullet = Instantiate(BulletPrefab, CanonOut.position, CanonOut.rotation);
                    Vector3 bulletEndTarget = _touchPosition + Vector3.up * -_depthOffset;
                    bullet.GetComponent<Michael.Scripts.Enemy.Bullet>()
                        .SetTrajectoryParameters(CanonOut.position, bulletEndTarget, _offset.position);
                    // Définir la direction du boulet pour correspondre à la direction du Canon
                    bullet.transform.forward = Canon.forward;

                    // Mettre à jour le dernier temps de tir
                    _lastShootTime = Time.time;

                    // Réinitialiser le slider à la valeur minimale
                    //CoolDownSlider.value = 0;
                    CoolDownImage.fillAmount = 0;
                }
            }
            else
            {
                // SFX
                SoundManager.PlaySound(SoundType.ShootFail);
            }
        }

        
        void UpdateCoolDownImage()
        {
            float timeSinceLastShot = Time.time - _lastShootTime;
            CoolDownImage.fillAmount = Mathf.Clamp(timeSinceLastShot, 0, ShootCooldown) / ShootCooldown;
        }
        /*void UpdateCoolDownSlider()
        {
            float timeSinceLastShot = Time.time - _lastShootTime;
            
            CoolDownSlider.value = Mathf.Clamp(timeSinceLastShot, 0, ShootCooldown);
        }*/

        void CalculateOffset(float distanceToTarget)
        {
            float offsetHeight = Mathf.Lerp(_touchPosition.y, Canon.position.y * _heightOffsetDelta,
                (distanceToTarget - minDistance) / (maxDistance - minDistance));
            
            _offset.position = new Vector3(_touchPosition.x, offsetHeight, _touchPosition.z);
        }

        void GetExplosiveBarrelTrajectory()
        {
            float offsetHeight = Mathf.Lerp(_touchPosition.y, BarrelOut.position.y * _barrelHeightOffsetDelta,
                (Vector3.Distance(BarrelOut.position, _touchPosition) - minDistance) / (maxDistance - minDistance));
            BarrelOffset.position = new Vector3(_touchPosition.x, offsetHeight, _touchPosition.z);
            // Calculez la direction du canon vers la position touchée
            Vector3 direction = (BarrelOffset.position - BarrelOut.position).normalized;

            // Interpolez la direction du tonneaux en utilisant le delta time et la vitesse
            BarrelOut.forward = Vector3.Lerp(BarrelOut.forward, direction, AimSpeed * Time.deltaTime);
            
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

       

     /*   private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_offset.position, 2f);
        }*/
        
        // Arme bonus : tonneaux explosifs
    }
}