using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    [SerializeField]
    void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {


        });
    }

  
}
