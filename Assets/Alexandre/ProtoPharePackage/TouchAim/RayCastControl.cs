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

    private Vector3 targetPosition;

    public float shootCooldown = 2.0f; // Cooldown time in seconds
    private float lastShootTime = 0f; // Time when the last shot was fired

    // Références pour le tir
    public GameObject BulletPrefab;
    public Transform CanonOut;
    // Ref au Canon
    public Transform Canon;
    // Reference CoolDown slider
    public Slider CoolDownSlider;

    // Paramètres de la courbe de Bézier
    public float minHeight = 2f; // Hauteur minimale pour les courtes distances
    public float maxHeight = 20f; // Hauteur maximale pour les longues distances
    public float maxDistance = 50f; // Distance maximale pour laquelle la hauteur est ajustée

    private Vector3 initialBulletDirection;
    private float maxHeightForDistance;

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
            targetPosition = Vector3.Lerp(touchPosition, direction, AimSpeed * Time.deltaTime);

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

    void UpdateCanonDirection()
    {
        if (touchPosition != Vector3.zero)
        {
            CalculateCanonDirectionAndHeight();

            // Interpoler la direction du canon en utilisant le delta time et la vitesse
            Canon.forward = Vector3.Lerp(Canon.forward, initialBulletDirection, AimSpeed * Time.deltaTime);
        }
    }

    public void ShootWithCooldown()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            // Instancier le boulet
            GameObject bullet = Instantiate(BulletPrefab, CanonOut.position, CanonOut.rotation);

            // Définir la direction du boulet pour correspondre à la direction du Canon
            bullet.transform.forward = Canon.forward;

            // Définir la position cible pour le boulet
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript.targetPosition = targetPosition;

            // Initialiser la trajectoire avec la hauteur ajustée
            bulletScript.InitializeTrajectory(minHeight, maxHeightForDistance, maxDistance);

            // Mettre à jour le dernier temps de tir
            lastShootTime = Time.time;

            // Réinitialiser le slider à la valeur minimale
            CoolDownSlider.value = 0;
        }
    }

    private void CalculateCanonDirectionAndHeight()
    {
        // Calculer la distance entre la position de départ et la position cible
        float distance = Vector3.Distance(CanonOut.position, touchPosition);

        // Calculer la hauteur maximale en fonction de la distance
        maxHeightForDistance = CalculateMaxHeight(distance);

        // Calculer la direction initiale du boulet
        initialBulletDirection = CalculateInitialBulletDirection(CanonOut.position, touchPosition, maxHeightForDistance);
    }

    private float CalculateMaxHeight(float distance)
    {
        // Utiliser une fonction quadratique pour ajuster la hauteur en fonction de la distance
        float height = minHeight + (maxHeight - minHeight) * Mathf.Pow(distance / maxDistance, 2);
        return height;
    }

    private Vector3 CalculateInitialBulletDirection(Vector3 startPosition, Vector3 targetPosition, float maxHeight)
    {
        Vector3 controlPoint = (startPosition + targetPosition) / 2 + Vector3.up * maxHeight;
        Vector3 initialDirection = CalculateQuadraticBezierTangent(0, startPosition, controlPoint, targetPosition);
        return initialDirection;
    }

    private Vector3 CalculateQuadraticBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        Vector3 tangent = 2 * u * (p1 - p0) + 2 * t * (p2 - p1);
        return tangent.normalized;
    }
}
