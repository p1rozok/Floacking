using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameObject prefabSlot;
    [SerializeField] private Transform content;

    private void Start()
    {
        UIView();
        Inventory.Instance.OnChangedInventory += UIView;
    }
    

    private void OnDisable()
    {
        Inventory.Instance.OnChangedInventory -= UIView;
    }
    private void UIView()
    {
        foreach (Transform slot in content)
            Destroy(slot.gameObject);

        foreach (var bait in Inventory.Instance.Baits)
        {
            if (bait.Amount > 0)
            {
                GameObject slot= Instantiate(prefabSlot,content);
                
                BaitSlotPick baitSlotPick=slot.GetComponent<BaitSlotPick>();
                baitSlotPick.Initialize(bait);

                Image image= slot.GetComponent<Image>();
                image.sprite=bait.Sprite;
                Text textMeshPro= slot.GetComponentInChildren<Text>();
                textMeshPro.text=bait.Amount.ToString();
            }
        }
    }

  
}
