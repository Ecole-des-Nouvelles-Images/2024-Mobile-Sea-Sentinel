using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PhareController : MonoBehaviour
{
    [Header("Phare Settings")]
    public GameObject PhareLight;
    public GameObject CanonOut;
    public GameObject BulletPrefab;

    [Header("Canon Settings")]
    public float SpeedRotation = 15.0f; // Vitesse de rotation du phare
    public float elevationSpeed = 100f; // Vitesse d'élévation
    public float maxElevationAngle = 45f; // Angle maximum
    public float minElevationAngle = -45f; // Angle minimum

    private float elevationAngle = 0f; // État actuel de l'élévation
    private bool isElevating = false; // Indique si le canon monte
    private bool isLowering = false; // Indique si le canon descend

    private void Update()
    {
        // Rotation continue du phare
        PhareLight.transform.Rotate(Vector3.up, SpeedRotation * Time.deltaTime, Space.World);

        // Ajustement continu de l'élévation
        if (isElevating)
        {
            AdjustElevation(1f);
        }
        else if (isLowering)
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
        isElevating = true;
    }

    public void StopElevating()
    {
        isElevating = false;
    }

    public void StartLowering()
    {
        isLowering = true;
    }

    public void StopLowering()
    {
        isLowering = false;
    }

    private void AdjustElevation(float direction)
    {
        elevationAngle += direction * elevationSpeed * Time.deltaTime;
        elevationAngle = Mathf.Clamp(elevationAngle, minElevationAngle, maxElevationAngle);
        PhareLight.transform.localEulerAngles = new Vector3(-elevationAngle, PhareLight.transform.localEulerAngles.y, PhareLight.transform.localEulerAngles.z);
    }
}
