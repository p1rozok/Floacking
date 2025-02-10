using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ToggleMiniGame : MonoBehaviour
{
    public static ToggleMiniGame Instance;

    public static UnityEvent OnCath=new UnityEvent();
    public static UnityEvent OnWin=new UnityEvent();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public static void EndFish()
    {

        OnWin?.Invoke();
    }
    public static void StartFish()
    {

        OnCath.Invoke();
    }
}
