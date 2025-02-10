using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    
    [SerializeField ]private List<Bait> _baits=new List<Bait>();


    private Bait _currentBait;
    private bool _search=false;
    public List<Bait> Baits => _baits;

    public event Action OnChangedInventory; 
  
    private void Awake()
    {
        if (Instance == null)

        Instance = this;
        
        else

       Destroy(Instance);
    }
    private void Start()
    {
        Select(_baits[0]);
    

        ToggleMiniGame.OnWin.AddListener(() =>
        {
            RemoveSlot(_currentBait);

        });
       
    }

  
    public Bait CurrentBait()
    {
        return _currentBait;
    }

    public void AddSlot(Bait bait)
    {
        foreach (var item in _baits)
        {
            if (item.Id == bait.Id)
            {
                item.Amount++;
                _search = true;
            }   
        }

        if(!_search)
            _baits.Add(bait);

        OnChangedInventory?.Invoke();
        _search = false;
    }

    private void RemoveSlot(Bait bait)
    {
        if (bait.Amount<= 0)
            _baits.Remove(bait);

        else
            bait.Amount--;

        OnChangedInventory?.Invoke();
    }

     public void Select(Bait bait)
    { 
      _currentBait = bait;
    }

    public bool PosibleTrowh()
    {
        if (_currentBait.Amount<=0)
            return false;

        return true;
    }
}

