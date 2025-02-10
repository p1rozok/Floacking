using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher instance;

    [SerializeField] private CinemachineVirtualCamera mainCamera;
    /*[SerializeField] private CinemachineVirtualCamera fixedCamera;*/
    [SerializeField] private CinemachineVirtualCamera hookCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SwitchToMainCamera()
    {
        SetCameraPriority(mainCamera);
    }

    /*public void SwitchToFixedCamera()
    {
        SetCameraPriority(fixedCamera);
    }*/

    public void SwitchToHookCamera(Transform hookTransform)
    {
        hookCamera.Follow = hookTransform;
        SetCameraPriority(hookCamera);
    }

    public void SwitchToPlayerCamera()
    {
        SetCameraPriority(playerCamera);
    }

    private void SetCameraPriority(CinemachineVirtualCamera cameraToActivate)
    {
        mainCamera.Priority = 5;
       /* fixedCamera.Priority = 5;*/
        hookCamera.Priority = 5;
        playerCamera.Priority = 5;

        cameraToActivate.Priority = 10;
    }
}
