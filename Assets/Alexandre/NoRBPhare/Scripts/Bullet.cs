using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 offset;
    private float journeyLength;
    private float startTime;
    public float speed = 10f; // Vitesse de la balle
    
    private bool _tracjetorySetted = false;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, endPosition);
    }

    void Update()
    {
        if (!_tracjetorySetted) return;
        float time = (Time.time - startTime) * speed;
        transform.position = GetBezierPoints(startPosition, endPosition, offset, time);
    }

    public void SetTrajectoryParameters(Vector3 start, Vector3 end, Vector3 newOffset)
    {
        startPosition = start;
        endPosition = end;
        offset = newOffset;
        _tracjetorySetted = true;
    }

    private Vector3 GetBezierPoints(Vector3 start, Vector3 end, Vector3 offset, float time)
    {
        Vector3 startToOffset = Vector3.Lerp(start, offset, time);
        Vector3 offsetToEnd = Vector3.Lerp(offset, end, time);
        return Vector3.Lerp(startToOffset, offsetToEnd, time);
    }


    void OnTriggerEnter(Collider other)
    {
        // Détruire le projectile lorsqu'il entre en collision avec un autre objet
        Destroy(gameObject);
    }
}