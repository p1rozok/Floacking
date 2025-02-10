using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bait",menuName ="Bait/Create new bait")]


[System.Serializable]
public class Bait :ScriptableObject
{
    public int Id;
    public string BaitType;
    public string Type;
    public Sprite Sprite;
    public int Amount = 1;


}
