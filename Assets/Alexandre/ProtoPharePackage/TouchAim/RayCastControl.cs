using UnityEngine;

public class RayCastControl : MonoBehaviour
{
    public GameObject RayCastUI;
    // Définissez le masque de couche pour la couche "Interactable"
    public LayerMask interactableLayer;

    // Référence à la lumière
    public GameObject phareLight;

    public float AimSpeed = 5f;

    // Position touchée
    private Vector3 touchPosition;

    public float shootCooldown = 2.0f; // Cooldown time in seconds
    private float lastShootTime = 0f; // Time when the last shot was fired

    // Références pour le tir
    public GameObject BulletPrefab;
    public Transform CanonOut;

    void Start()
    {
        RayCastUI.SetActive(true);
    }
    void Update()
    {
        // Vérifiez s'il y a des touches sur l'écran
        if (Input.touchCount > 0)
        {
            // Obtenez le premier toucher
            Touch touch = Input.GetTouch(0);
            Debug.Log("Touch phase : " + touch.phase);

            // Vérifiez si le toucher est une phase de début (quand le joueur commence à toucher l'écran)
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
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
                }
            }
        }
    }

    void UpdateLightDirection()
    {
        if (phareLight != null && touchPosition != Vector3.zero)
        {
            // Calculez la direction de la lumière vers la position touchée
            Vector3 direction = (touchPosition - phareLight.transform.position).normalized;

            // Interpolez la direction de la lumière en utilisant le delta time et la vitesse
            phareLight.transform.forward = Vector3.Lerp(phareLight.transform.forward, direction, AimSpeed * Time.deltaTime);
        }
    }

    public void ShootWithCooldown()
    {
        if (Time.time >= lastShootTime + shootCooldown)
        {
            // Instantiate the bullet prefab
            GameObject bullet = Instantiate(BulletPrefab, CanonOut.position, CanonOut.rotation);

            // Set the bullet's direction to match the direction of phareLight
            bullet.transform.forward = phareLight.transform.forward;

            // Update the last shoot time
            lastShootTime = Time.time;
        }
    }
}