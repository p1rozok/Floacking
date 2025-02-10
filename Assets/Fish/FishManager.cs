using UnityEngine;

public class FishManager : MonoBehaviour
{
    public FishData[] fishTypes;
    public GameObject fishPrefab;
    public int numberOfFish = 1000;
    public Rect habitatArea = new Rect(-50, -50, 100, 100);
    public float bufferZone = 5f;

    private FishTickManager fishTickManager;

    private void Start()
    {
        fishTickManager = FindObjectOfType<FishTickManager>();
        if (fishTickManager == null)
        {
            Debug.LogError("FishTickManager not found in the scene.");
            return;
        }

        for (int i = 0; i < numberOfFish; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(habitatArea.xMin, habitatArea.xMax),
                Random.Range(habitatArea.yMin, habitatArea.yMax)
            );

            GameObject fishObj = Instantiate(fishPrefab, pos, Quaternion.identity, transform);

            FlockingFish fish = fishObj.GetComponent<FlockingFish>();
            if (fish == null)
            {
                Debug.LogError("The prefab does not contain a FlockingFish component.");
                continue;
            }

            FishData fishData = fishTypes[Random.Range(0, fishTypes.Length)];
            FishData fishDataCopy = Instantiate(fishData);

            fishDataCopy.alignmentWeight += Random.Range(-0.1f, 0.1f);
            fishDataCopy.cohesionWeight += Random.Range(-0.1f, 0.1f);
            fishDataCopy.separationWeight += Random.Range(-0.1f, 0.1f);

            fish.Initialize(fishDataCopy, habitatArea, bufferZone, fishTickManager, i);
        }
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

