using UnityEngine;

[CreateAssetMenu(fileName = "New Fish Data", menuName = "Fish/FishData")]
public class FishData : ScriptableObject
{
    public Sprite fishSprite;
    public Color color;
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
}
