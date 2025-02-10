using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwithCameraToBait : MonoBehaviour
{
    [SerializeField] private CameraSwitcher camera;
    [SerializeField] private ThrowHook hook;
    [SerializeField] private GameObject bait;
 
    private void OnEnable()
    {
        hook.OnTrowHook += SetCamera;
    }

    private void OnDisable()
    {
        hook.OnTrowHook -= SetCamera;
    }


    private void SetCamera()
    {

        camera.SwitchToHookCamera(bait.transform);


    }
}
