using Alexandre;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RayCastControl : MonoBehaviour
{
    public bool autoShoot = false;
    public GameObject RayCastUI;

    // Définissez le masque de couche pour la couche "Interactable"
    public LayerMask interactableLayer;

    // Référence à la lumière
    public GameObject phareLight;

    public float AimSpeed = 5f;
    public RectTransform crosshairUI;

    public GameObject CrosshairPrefab;
    private GameObject crosshairInstance;
    // Position touchée
    private Vector3 touchPosition;

    public float shootCooldown = 2.0f; // Cooldown time in seconds
    private float lastShootTime = 0f; // Time when the last shot was fired

    // Références pour le tir
    public GameObject BulletPrefab;
    public Transform CanonOut;

    // Reference CoolDown slider
    public Slider CoolDownSlider;

    void Start()
    {
        RayCastUI.SetActive(true);
        CoolDownSlider.maxValue = shootCooldown;
        CoolDownSlider.value = 0; // Initialiser à la valeur minimale
        crosshairInstance = Instantiate(CrosshairPrefab);
    }

    void Update()
    {
        UpdateLightDirection();
        UpdateCoolDownSlider();

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
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
                {
                    // Si le raycast touche quelque chose, obtenez la position
                    touchPosition = hit.point;
                    Debug.Log("Position touchée : " + touchPosition);

                    // Mettez à jour la direction de la lumière
                    UpdateLightDirection();
                    UpdateCrosshairPosition(hit.point);
                }
            }

            if (touch.phase == TouchPhase.Ended && autoShoot)
            {
                ShootWithCooldown();
            }
        }
    }

    void UpdateCoolDownSlider()
    {
        float timeSinceLastShot = Time.time - lastShootTime;
        CoolDownSlider.value = Mathf.Clamp(timeSinceLastShot, 0, shootCooldown);
    }

    void UpdateLightDirection()
    {
        if (phareLight != null && touchPosition != Vector3.zero)
        {
            // Calculez la direction de la lumière vers la position touchée
            Vector3 direction = (touchPosition - phareLight.transform.position).normalized;
            //
            Vector3 targetPosition = Vector3.Lerp(touchPosition, direction, AimSpeed * Time.deltaTime);

            // Interpolez la direction de la lumière en utilisant le delta time et la vitesse
            phareLight.transform.forward =
                Vector3.Lerp(phareLight.transform.forward, direction, AimSpeed * Time.deltaTime);
        }
    }

    void UpdateCrosshairPosition(Vector3 hitPoint)
    {
        crosshairInstance.transform.position = hitPoint;
        // Vector2 screenPoint = Camera.main.WorldToScreenPoint(hitPoint);
        // crosshairUI.position = screenPoint;
    }

    public void ShootWithCooldown()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            // Instantiate the bullet prefab
            GameObject bullet = Instantiate(BulletPrefab, CanonOut.position, CanonOut.rotation);

            // Set the bullet's direction to match the direction of phareLight
            bullet.transform.forward = phareLight.transform.forward;

            // Set the target position for the bullet
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.targetPosition = touchPosition;

            // Update the last shoot time
            lastShootTime = Time.time;

            // Réinitialiser le slider à la valeur minimale
            CoolDownSlider.value = 0;
        }
    }
}