using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHControls : MonoBehaviour
{
    [SerializeField] private Transform _posStart;
    [SerializeField] private Transform _posEnd;
    [SerializeField] private Transform _posOffset;
    [SerializeField] private Transform _ref;
    [SerializeField] private float _time = 0.5f;
    
    [SerializeField] private float _minAimDistance = 5f;
    [SerializeField] private float _maxAimDistance = 50f;
    

    // void Update()
    // {
    //     Debug.Log(Mathf.Lerp(_minAimDistance, _maxAimDistance, _time));
    // }
    private Vector3 GetBezierPoints(Vector3 start , Vector3 end, Vector3 offset, float time) {
        
        Vector3 startToOffset = Vector3.Lerp(start, offset, time);
        Vector3 offsetToEnd = Vector3.Lerp(offset,end,  time);
        return Vector3.Lerp(startToOffset, offsetToEnd, time);
    }
    private void OnDrawGizmos() {
        // if (_ref == null) return;
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere( _ref.position, _noclickZone );
        

            if (_posStart == null || _posEnd == null || _posOffset == null) return;

            // Calculate the direction vector and normalize it
            Vector3 direction = (_posEnd.position - _posStart.position).normalized;

            // Calculate the distance between _posStart and _posEnd
            float distance = Vector3.Distance(_posStart.position, _posEnd.position);

            float distance2 = _posEnd.position.z - _minAimDistance;

            // Use Mathf.Lerp to adjust the height of _posOffset
            float offsetHeight = Mathf.Lerp(_posEnd.position.y,_posStart.position.y, distance2/_maxAimDistance);

            // Set the _posOffset position
            _posOffset.position = _posStart.position + direction * distance;
            _posOffset.position = new Vector3(_posOffset.position.x, offsetHeight, _posOffset.position.z);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_posOffset.position, 2f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(GetBezierPoints(_posStart.position, _posEnd.position, _posOffset.position, _time), 2f);
        }
}
