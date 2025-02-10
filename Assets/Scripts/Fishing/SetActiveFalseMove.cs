using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveFalseMove : MonoBehaviour
{
    [SerializeField] private MoveBoat moveBoat;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ThrowHook throwHook;
    private void OnEnable()
    {
        throwHook.OnTrowHook += SetActiveFalse;

    }

    private void OnDisable()
    {
        throwHook.OnTrowHook -= SetActiveFalse;

    }

    private void SetActiveFalse()
    {
        moveBoat.enabled = false;
        playerController.enabled = false;
    }
}
  
 
    

