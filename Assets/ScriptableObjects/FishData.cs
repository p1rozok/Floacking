using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Data", menuName = "Fish/FishData")]
public class FishData : ScriptableObject
{
    
    public enum FishRarity
    {
        Common,
        Uncommon,
        Rare,
        Ultrarare,
        Legendary,
        
    }
    public FishRarity rarity;
    
    public Sprite fishSprite;
    public Color color;
    public float spriteRotation;
    public float rotationSpeed = 2f;
    public float neighborDistance = 1.5f;
    public float avoidanceRadius = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;
    public float separationWeight = 1.5f;
    public float maxSpeed = 2f;
    public float maxForce = 0.5f;
    public string species;
    
    public bool avoidOtherSpecies = true;

    public float rare = 1f;
    
    
    
    public string preferredBait;
    public float detectionRadius = 5f;
    public float baitAttractionWeight = 1f;
    public float playerAvoidanceWeight = 1f;
    public float attractionWeight = 1f;
    public string preferredBaitType;
    

   


}
