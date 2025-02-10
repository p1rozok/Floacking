using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class FishTickManager : MonoBehaviour
{
    public float behaviorTickInterval = 0.02f;
    public float neighborTickInterval = 0.1f;
    private List<FlockingFish> allFish = new List<FlockingFish>();

    private NativeArray<Vector2> positions;
    private NativeArray<Vector2> velocities;
    private NativeArray<int> neighborCounts;
    private NativeArray<int> neighborOffsets;
    private NativeArray<int> neighborIndices;

    void Start()
    {
        StartCoroutine(BehaviorTickCoroutine());
        StartCoroutine(NeighborTickCoroutine());
    }

    public void RegisterFish(FlockingFish fish)
    {
        if (!allFish.Contains(fish))
        {
            allFish.Add(fish);
        }
    }

    private IEnumerator BehaviorTickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(behaviorTickInterval);
            UpdateAllFishBehavior();
        }
    }

    private IEnumerator NeighborTickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(neighborTickInterval);
            UpdateAllFishNeighbors();
        }
    }

    private void UpdateAllFishBehavior()
    {
        foreach (var fish in allFish)
        {
            fish.UpdateBehavior();
        }
    }

    private void UpdateAllFishNeighbors()
    {
        int fishCount = allFish.Count;

        if (positions.IsCreated) positions.Dispose();
        if (velocities.IsCreated) velocities.Dispose();
        if (neighborCounts.IsCreated) neighborCounts.Dispose();
        if (neighborOffsets.IsCreated) neighborOffsets.Dispose();
        if (neighborIndices.IsCreated) neighborIndices.Dispose();

        positions = new NativeArray<Vector2>(fishCount, Allocator.TempJob);
        velocities = new NativeArray<Vector2>(fishCount, Allocator.TempJob);
        neighborCounts = new NativeArray<int>(fishCount, Allocator.TempJob);

        for (int i = 0; i < fishCount; i++)
        {
            positions[i] = allFish[i].transform.position;
            velocities[i] = allFish[i].velocity;
        }

        float neighborRadius = allFish[0].FishData.neighborDistance;
        float neighborRadiusSqr = neighborRadius * neighborRadius;

        NeighborCalculationJob neighborJob = new NeighborCalculationJob
        {
            positions = positions,
            neighborRadiusSqr = neighborRadiusSqr,
            neighborCounts = neighborCounts,
            maxNeighbors = fishCount,
            neighborList = new NativeArray<int>(fishCount * fishCount, Allocator.TempJob) 
        };

        JobHandle neighborJobHandle = neighborJob.Schedule();
        neighborJobHandle.Complete();

        neighborOffsets = new NativeArray<int>(fishCount, Allocator.TempJob);
        int totalNeighbors = 0;
        for (int i = 0; i < fishCount; i++)
        {
            neighborOffsets[i] = totalNeighbors;
            totalNeighbors += neighborCounts[i];
        }

        neighborIndices = new NativeArray<int>(totalNeighbors, Allocator.TempJob);
        NativeArray<int>.Copy(neighborJob.neighborList, neighborIndices, totalNeighbors);

        neighborJob.neighborList.Dispose();

        for (int i = 0; i < fishCount; i++)
        {
            allFish[i].SetData(positions, velocities, neighborCounts, neighborIndices, neighborOffsets);
        }
    }

    [BurstCompile]
    struct NeighborCalculationJob : IJob
    {
        [ReadOnly] public NativeArray<Vector2> positions;
        public float neighborRadiusSqr;

        public NativeArray<int> neighborCounts;
        public NativeArray<int> neighborList;

        public int maxNeighbors;

        public void Execute()
        {
            int fishCount = positions.Length;
            int neighborListIndex = 0;

            for (int i = 0; i < fishCount; i++)
            {
                int count = 0;
                Vector2 position = positions[i];

                for (int j = 0; j < fishCount; j++)
                {
                    if (i == j) continue;

                    if ((positions[j] - position).sqrMagnitude < neighborRadiusSqr)
                    {
                        if (neighborListIndex < neighborList.Length)
                        {
                            neighborList[neighborListIndex] = j;
                            neighborListIndex++;
                            count++;
                        }
                    }
                }

                neighborCounts[i] = count;
            }
        }
    }

    private void OnDestroy()
    {
        
    }
}

