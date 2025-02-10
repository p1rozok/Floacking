using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class LineHookAndBait : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject bait;
    [SerializeField] private GameObject hook;

    void Start()
    {
        line.positionCount = 2;   
    }

    
    void Update()
    {
        SetLine();    
    }
    private void SetLine() 
    { 

    line.SetPosition(0,hook.transform.position);
    line.SetPosition(1,bait.transform.position);

    }
}
