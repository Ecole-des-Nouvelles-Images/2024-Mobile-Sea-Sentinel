using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(TrajectoryPredictor))]
public class ProjectileThrow : MonoBehaviour
{
    TrajectoryPredictor trajectoryPredictor;

    [SerializeField]
    Rigidbody objectToThrow;

    [SerializeField, Range(0.0f, 250.0f)]
    float force;

    [SerializeField]
    Transform StartPosition;

    public GameObject Visor;

    public float ShootCoolDown = 2f;
    private float lastShootTime;
    
    public Slider CoolDownSlider;
    void OnEnable()
    {
        trajectoryPredictor = GetComponent<TrajectoryPredictor>();

        if (StartPosition == null)
            StartPosition = transform;
        
        CoolDownSlider.maxValue = ShootCoolDown;
        CoolDownSlider.value = 0; // Initialiser à la valeur minimale


    }

    void Update()
    {
        Predict();
        UpdateCoolDownSlider();

    }

    void Predict()
    {
        trajectoryPredictor.PredictTrajectory(ProjectileData());
    }

    void UpdateCoolDownSlider()
    {
        float timeSinceLastShot = Time.time - lastShootTime;
        CoolDownSlider.value = Mathf.Clamp(timeSinceLastShot, 0, ShootCoolDown);
    }
    ProjectileProperties ProjectileData()
    {
        ProjectileProperties properties = new ProjectileProperties();
        Rigidbody r = objectToThrow.GetComponent<Rigidbody>();

        properties.direction = ( Visor.transform.position - StartPosition.position).normalized;
        properties.initialPosition = StartPosition.position;
        properties.initialSpeed = force;
        properties.mass = r.mass;
        properties.drag = r.drag;

        return properties;
    }

    public void ThrowObject()
    {
        if (Time.time >= lastShootTime + ShootCoolDown)
        {
            Rigidbody thrownObject = Instantiate(objectToThrow, StartPosition.position, Quaternion.identity);
            thrownObject.AddForce(StartPosition.forward * force, ForceMode.Impulse);
            lastShootTime = Time.time;
            CoolDownSlider.value = 0;

        }

        

    }
}