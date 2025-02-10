using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitSlotPick : MonoBehaviour
{
    private Outline _outline;
    private Bait _bait;

    public void Initialize(Bait bait)
    {
        _bait = bait;

        Button button = GetComponent<Button>();
        button.onClick.AddListener(Select);
        _outline = GetComponent<Outline>();
    }

    private void Select()
    {
        Inventory.Instance.Select(_bait);
        _outline.enabled = true;
        foreach (Transform outline in transform.parent)
        {
            if (outline != transform)
            {
                Outline otherOutline = outline.GetComponent<Outline>();
                if (otherOutline) 
                otherOutline.enabled = false;


            }
        }
    }
}
