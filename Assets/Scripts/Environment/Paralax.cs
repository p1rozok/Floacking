using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float paralaxSpeed;

    private Vector3 _lastPositionCamera;
 
    private void Start()
    {
        _lastPositionCamera = mainCamera.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = mainCamera.transform.position - _lastPositionCamera;
        transform.position += delta* paralaxSpeed;

        _lastPositionCamera = mainCamera.transform.position;
    }

 
}
