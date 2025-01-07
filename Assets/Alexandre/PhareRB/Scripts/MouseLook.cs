using UnityEngine;
using UnityEngine.EventSystems;

public class MouseLook : MonoBehaviour
{
    #region variables
    public Vector2 clampInDegrees = new Vector2(360f, 180f);
    public Vector2 sensitivity = new Vector2(0.1f, 0.1f);
    public Vector2 smoothing = new Vector2(1f, 1f);

    public bool lockCursor;
    public GameObject characterBody;

    // Référence à la couche interactable
    public LayerMask interactableLayer;

    public GameObject Visor;

    // Position touchée
    private Vector3 touchPosition;
    private Vector3 targetDirection;
    #endregion

    void Start()
    {
        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;
    }

    void LateUpdate()
    {
        if (lockCursor)
            Cursor.lockState = CursorLockMode.Locked;

        // Vérifiez s'il y a des touches sur l'écran
        if (Input.touchCount > 0)
        {
            // Obtenez le premier toucher
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            // Vérifiez si le toucher est une phase de début (quand le joueur commence à toucher l'écran)
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved ||
                touch.phase == TouchPhase.Stationary)
            {
                // Convertissez la position du toucher en un rayon
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Effectuez un raycast avec le masque de couche
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
                {
                    // Si le raycast touche quelque chose, obtenez la position
                    touchPosition = hit.point;

                    // Mettez à jour la position du viseur
                    UpdateVisorPosition(hit.point);
                }
            }
        }

        // Orienter progressivement le characterBody vers le dernier point de visée
        AlignCharacterToVisor();
    }

    void UpdateVisorPosition(Vector3 hitPoint)
    {
        if (hitPoint != Vector3.zero)
        {
            // Déplacez le viseur à la position touchée
            Visor.transform.position = Vector3.Lerp(Visor.transform.position, hitPoint, Time.deltaTime * 5f);
        }
    }

    void AlignCharacterToVisor()
    {
        if (characterBody != null && Visor != null)
        {
            // Calculez la direction du viseur par rapport au characterBody
            Vector3 directionToVisor = (Visor.transform.position - characterBody.transform.position).normalized;

            // Calculez la rotation cible
            Quaternion targetRotation = Quaternion.LookRotation(directionToVisor);

            // Interpolez progressivement vers la rotation cible
            characterBody.transform.rotation = Quaternion.Slerp(characterBody.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
