using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishTickManager : MonoBehaviour
{
    public static FishTickManager Instance { get; private set; }

    public float neighborTickInterval = 0.1f;
    private readonly List<FlockingFish> _allFish = new List<FlockingFish>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StartCoroutine(NeighborTickCoroutine());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterFish(FlockingFish fish)
    {
        if (fish != null && !_allFish.Contains(fish))
        {
            _allFish.Add(fish);
        }
    }

    public void UnregisterFish(FlockingFish fish)
    {
        if (fish != null)
        {
            _allFish.Remove(fish);
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

    private void UpdateAllFishNeighbors()
    {
        for (int i = _allFish.Count - 1; i >= 0; i--)
        {
            if (_allFish[i] == null)
            {
                _allFish.RemoveAt(i);
            }
            else
            {
                _allFish[i].UpdateNeighbors();
            }
        }
    }
}