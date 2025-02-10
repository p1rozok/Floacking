using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(BoxCollider2D))]

public class FlockingFish : MonoBehaviour
{
    public float obstacleAvoidanceDistance = 1.0f;
    public float obstacleAvoidanceWeight = 1.0f;

    private FishData fishData;
    public FishData FishData => fishData;
    private string species;
    private Rect habitatArea;
    private float bufferZone;
    public Vector2 velocity;
    private Vector2 acceleration;
    private SpriteRenderer spriteRenderer;

    private NativeArray<Vector2> positions;
    private NativeArray<Vector2> velocities;
    private NativeArray<int> neighborCounts;
    private NativeArray<int> neighborIndices;
    private NativeArray<int> neighborOffsets;

    private int fishIndex;

    public void Initialize(FishData data, Rect habitatArea, float bufferZone, FishTickManager fishTickManager, int index)
    {
        fishData = data;
        this.habitatArea = habitatArea;
        this.bufferZone = bufferZone;
        this.fishIndex = index;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = fishData.fishSprite;
            spriteRenderer.color = fishData.color;
        }

        velocity = Random.insideUnitCircle.normalized * fishData.maxSpeed;
        acceleration = Vector2.zero;
        species = fishData.species;

        fishTickManager.RegisterFish(this);
    }

    public void UpdateBehavior()
    {
        if (fishData == null)
            return;

        acceleration = Vector2.zero;

        if (!positions.IsCreated || !velocities.IsCreated || !neighborCounts.IsCreated || !neighborIndices.IsCreated || !neighborOffsets.IsCreated)
            return;

        CalculateFlock();
        KeepInBounds();
        ObstacleAvoidance();

        ApplyMovement();
    }

    public void SetData(NativeArray<Vector2> positions, NativeArray<Vector2> velocities,
                        NativeArray<int> neighborCounts, NativeArray<int> neighborIndices, NativeArray<int> neighborOffsets)
    {
        this.positions = positions;
        this.velocities = velocities;
        this.neighborCounts = neighborCounts;
        this.neighborIndices = neighborIndices;
        this.neighborOffsets = neighborOffsets;
    }

    private void CalculateFlock()
    {
        if (fishIndex < 0 || fishIndex >= neighborCounts.Length)
            return;

        int neighborCount = neighborCounts[fishIndex];
        int offset = neighborOffsets[fishIndex];

        if (neighborCount == 0)
            return;

        Vector2 alignment = Vector2.zero;
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;

        Vector2 currentPosition = positions[fishIndex];
        Vector2 currentVelocity = velocities[fishIndex];

        for (int i = 0; i < neighborCount; i++)
        {
            int neighborArrayIndex = offset + i;

            if (neighborArrayIndex < 0 || neighborArrayIndex >= neighborIndices.Length)
                continue;

            int neighborIndex = neighborIndices[neighborArrayIndex];

            if (neighborIndex < 0 || neighborIndex >= positions.Length)
                continue;

            Vector2 neighborPosition = positions[neighborIndex];
            Vector2 neighborVelocity = velocities[neighborIndex];
            Vector2 toNeighbor = currentPosition - neighborPosition;
            float distance = toNeighbor.magnitude;

            alignment += neighborVelocity;
            cohesion += neighborPosition;

            if (distance > 0 && distance < fishData.avoidanceRadius)
            {
                Vector2 awayFromNeighbor = toNeighbor / (distance * distance);
                separation += awayFromNeighbor;
            }
        }

        alignment /= neighborCount;
        cohesion /= neighborCount;

        alignment = alignment.normalized * fishData.maxSpeed;
        alignment -= currentVelocity;
        alignment = Vector2.ClampMagnitude(alignment, fishData.maxForce);

        cohesion -= currentPosition;
        cohesion = cohesion.normalized * fishData.maxSpeed;
        cohesion -= currentVelocity;
        cohesion = Vector2.ClampMagnitude(cohesion, fishData.maxForce);

        separation = separation.normalized * fishData.maxSpeed;
        separation -= currentVelocity;
        separation = Vector2.ClampMagnitude(separation, fishData.maxForce);

        alignment *= fishData.alignmentWeight;
        cohesion *= fishData.cohesionWeight;
        separation *= fishData.separationWeight;

        acceleration += alignment + cohesion + separation;
    }

    private void ObstacleAvoidance()
    {
        float rayDistance = obstacleAvoidanceDistance;
        Vector2 direction = velocity.normalized;
        Vector2 rayOrigin = (Vector2)transform.position + direction * 0.1f;

        Debug.DrawRay(rayOrigin, direction * rayDistance, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance, LayerMask.GetMask("Obstacle"));
        if (hit.collider != null)
        {
            Vector2 hitNormal = hit.normal;
            Vector2 avoidForce = Vector2.Reflect(direction, hitNormal);
            avoidForce.Normalize();
            avoidForce *= fishData.maxForce * obstacleAvoidanceWeight;

            acceleration += avoidForce;
        }
    }

    private void KeepInBounds()
    {
        Vector2 position = transform.position;

        float xMin = habitatArea.xMin + bufferZone;
        float xMax = habitatArea.xMax - bufferZone;
        float yMin = habitatArea.yMin + bufferZone;
        float yMax = habitatArea.yMax - bufferZone;

        Vector2 toCenter = Vector2.zero;

        if (position.x < xMin)
        {
            toCenter.x = xMin - position.x;
        }
        else if (position.x > xMax)
        {
            toCenter.x = xMax - position.x;
        }

        if (position.y < yMin)
        {
            toCenter.y = yMin - position.y;
        }
        else if (position.y > yMax)
        {
            toCenter.y = yMax - position.y;
        }

        if (toCenter != Vector2.zero)
        {
            toCenter = toCenter.normalized * fishData.maxForce * 2f;
            acceleration += toCenter;
            velocity = Vector2.ClampMagnitude(velocity, fishData.maxSpeed * 0.5f);
        }
    }

    private void ApplyMovement()
    {
        velocity += acceleration * Time.deltaTime;
        velocity = Vector2.ClampMagnitude(velocity, fishData.maxSpeed);

        Vector2 newPosition = (Vector2)transform.position + velocity * Time.deltaTime;
        transform.position = newPosition;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
}