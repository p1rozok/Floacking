using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveTrueMove : MonoBehaviour
{
    [SerializeField] private MoveBoat moveBoat;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ThrowHook throwHook;



    private void OnEnable()
    {
        throwHook.OnTrowRetrieve += SetActiveTrue;
    }

    private void OnDisable()
    {
        throwHook.OnTrowRetrieve -= SetActiveTrue;

    }
    private void SetActiveTrue()
    {
        moveBoat.enabled = true;
        playerController.enabled = true;
    }
}
