using UnityEngine;
using System.Collections.Generic;

public class FishManager : MonoBehaviour
{
    public FishData[] fishTypes;
    public GameObject fishPrefab;
    public int numberOfFish = 1000;
    public Rect habitatArea = new Rect(-50, -50, 100, 100);
    public float bufferZone = 5f;
    public float rareFishThreshold = 0.3f;

    private SpatialGrid _spatialGrid;
    private FishTickManager _fishTickManager;
    private List<FlockingFish> _fishes = new List<FlockingFish>();

    private float spawnInterval = 0f;
    private float spawnTimer = 0f;

    private void Start()
    {
        _fishTickManager = FishTickManager.Instance;
        if (_fishTickManager == null)
        {
            GameObject tickManagerObject = new GameObject("FishTickManager");
            _fishTickManager = tickManagerObject.AddComponent<FishTickManager>();
        }

        float cellSize = 5f;
        Vector2 originPosition = new Vector2(habitatArea.xMin, habitatArea.yMin);

        _spatialGrid = new SpatialGrid(cellSize, originPosition);

        PopulationManager.Instance?.ResetPopulation();

        for (int i = 0; i < numberOfFish; i++)
        {
            SpawnFish();
            PopulationManager.Instance?.IncreasePopulation(1f);
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;

            if (PopulationManager.Instance != null)
            {
                if (PopulationManager.Instance.CurrentPopulationPercentage < 30f)
                {
                    FishData rareFishData = SelectRareFishData();
                    if (rareFishData != null)
                    {
                        SpawnFish(rareFishData);
                        PopulationManager.Instance.IncreasePopulation(1f);
                    }
                }
                else if (PopulationManager.Instance.CanSpawnFish() &&
                         _fishes.Count < PopulationManager.Instance.maxPopulation)
                {
                    if (_fishes.Count < PopulationManager.Instance.maxPopulation)
                    {
                        SpawnFish();
                        PopulationManager.Instance.IncreasePopulation(1f);
                    }
                }
            }
        }
    }

    private void SpawnFish(FishData fishData = null)
    {
        Vector2 pos = new Vector2(
            Random.Range(habitatArea.xMin, habitatArea.xMax),
            Random.Range(habitatArea.yMin, habitatArea.yMax)
        );

        GameObject fishObj = Instantiate(fishPrefab, pos, Quaternion.identity, transform);

        FlockingFish fish = fishObj.GetComponent<FlockingFish>();
        if (fish == null)
        {
            Destroy(fishObj);
            return;
        }

        if (fishData == null)
        {
            fishData = SelectFishDataByRarity();
        }

        FishData fishDataCopy = Instantiate(fishData);

        fishDataCopy.alignmentWeight += Random.Range(-0.1f, 0.1f);
        fishDataCopy.cohesionWeight += Random.Range(-0.1f, 0.1f);
        fishDataCopy.separationWeight += Random.Range(-0.1f, 0.1f);

        fish.Initialize(fishDataCopy, _spatialGrid, habitatArea, bufferZone, _fishTickManager, this);

        _fishes.Add(fish);
    }

    public void RemoveFish(FlockingFish fish)
    {
        if (fish == null)
        {
            return;
        }

        if (_fishes.Contains(fish))
        {
            _fishes.Remove(fish);
        }
    }

    private FishData SelectFishDataByRarity()
    {
        float totalWeight = 0f;

        foreach (var fishType in fishTypes)
        {
            totalWeight += 1f / Mathf.Max(fishType.rare, 0.01f);
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var fishType in fishTypes)
        {
            cumulativeWeight += 1f / Mathf.Max(fishType.rare, 0.01f);
            if (randomValue <= cumulativeWeight)
            {
                return fishType;
            }
        }

        return fishTypes[fishTypes.Length - 1];
    }

    private FishData SelectRareFishData()
    {
        List<FishData> rareFishList = new List<FishData>();

        foreach (var fishType in fishTypes)
        {
            if (fishType.rare >= rareFishThreshold)
            {
                rareFishList.Add(fishType);
            }
        }

        if (rareFishList.Count > 0)
        {
            int randomIndex = Random.Range(0, rareFishList.Count);
            return rareFishList[randomIndex];
        }

        Debug.LogWarning("[FishManager] No rare fish found with the required threshold.");
        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(habitatArea.center, habitatArea.size);

        Rect bufferArea = new Rect(
            habitatArea.xMin + bufferZone,
            habitatArea.yMin + bufferZone,
            habitatArea.width - 2 * bufferZone,
            habitatArea.height - 2 * bufferZone
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bufferArea.center, bufferArea.size);
    }
}
