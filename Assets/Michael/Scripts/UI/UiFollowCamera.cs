using System;
using UnityEngine;

namespace Michael.Scripts.UI
{
    public class UiFollowCamera : MonoBehaviour
    {
       private Camera _mainCamera;

       private void Start()
       {
           _mainCamera = Camera.main;
       }

       void LateUpdate() {
            transform.LookAt(transform.position +  _mainCamera.transform.rotation * Vector3.forward,  _mainCamera.transform.rotation * Vector3.up);
        }
    }
}