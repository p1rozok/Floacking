using UnityEngine;
using System.Collections.Generic;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudPrefabs;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private Vector2 direction = Vector2.left;
    [SerializeField] private int initialCloudCount = 5;

    private List<GameObject> cloudPool;
    private float timer;
    private float screenLeft;
    private float screenRight;
    private float screenTop;
    private float screenBottom;
    private float topQuarter;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        CreateCloudPool();
        UpdateScreenBounds();
        SpawnInitialClouds();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            UpdateScreenBounds();
            SpawnCloud();
            timer = spawnInterval;
        }
    }

    void SpawnInitialClouds()
    {
        for (int i = 0; i < initialCloudCount; i++)
        {
            GameObject cloud = GetRandomInactiveCloud();
            if (cloud == null)
                break;

            float spawnX = Random.Range(screenLeft, screenRight);
            float spawnY = Random.Range(topQuarter, screenTop); 

            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);
            cloud.transform.position = spawnPosition;
            cloud.SetActive(true);

            CloudMovement cloudMovement = cloud.GetComponent<CloudMovement>();
            cloudMovement.SetDirection(direction);
            cloudMovement.Speed = Random.Range(minSpeed, maxSpeed);
            cloudMovement.SetMovementArea(screenLeft, screenRight);
        }
    }

    void SpawnCloud()
    {
        GameObject cloud = GetRandomInactiveCloud();
        if (cloud == null)
            return;

        float spawnY = Random.Range(topQuarter, screenTop); 

        Vector3 spawnPosition;

        if (direction == Vector2.right)
        {
            spawnPosition = new Vector3(screenLeft - 1, spawnY, 0);
        }
        else
        {
            spawnPosition = new Vector3(screenRight + 1, spawnY, 0);
        }

        cloud.transform.position = spawnPosition;
        cloud.SetActive(true);

        CloudMovement cloudMovement = cloud.GetComponent<CloudMovement>();
        cloudMovement.SetDirection(direction);
        cloudMovement.Speed = Random.Range(minSpeed, maxSpeed);
        cloudMovement.SetMovementArea(screenLeft, screenRight);
    }

    void UpdateScreenBounds()
    {
        float cameraHeight = 2f * cam.orthographicSize;
        float cameraWidth = cameraHeight * cam.aspect;

        screenLeft = cam.transform.position.x - cameraWidth / 2;
        screenRight = cam.transform.position.x + cameraWidth / 2;
        screenTop = cam.transform.position.y + cameraHeight / 2;
        screenBottom = cam.transform.position.y - cameraHeight / 2;
        topQuarter = screenTop - (cameraHeight * 0.25f);
    }

    void CreateCloudPool()
    {
        
        
        cloudPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject cloud = Instantiate(cloudPrefabs[i % cloudPrefabs.Length], transform);
            cloud.SetActive(false);
            cloudPool.Add(cloud);
        }
    }

    GameObject GetRandomInactiveCloud()
    {
        List<GameObject> inactiveClouds = new List<GameObject>();

        foreach (GameObject cloud in cloudPool)
        {
            if (!cloud.activeInHierarchy)
            {
                inactiveClouds.Add(cloud);
            }
        }

        if (inactiveClouds.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveClouds.Count);
            return inactiveClouds[randomIndex];
        }

        return null;
    }
}
