using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bait",menuName ="Bait/Create new bait")]
public class Bait :ScriptableObject
{
    public string Type;
    public Sprite Sprite;
    public int Amount;
}
