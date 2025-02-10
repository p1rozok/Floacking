using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    
    [SerializeField] private Bait worm;
   
    private List<Bait> _baits=new List<Bait>();
    private int _currentIndexBait;

    public Bait CurrentBait()
    {
        return _baits[_currentIndexBait];
    }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _baits.Add(worm);
        _currentIndexBait = 0;

    }
}

