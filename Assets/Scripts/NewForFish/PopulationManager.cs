using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager Instance;

    public float maxPopulation = 1000f;
    public float lowPopulationThreshold = 50f;
    private float currentPopulation = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public float CurrentPopulationPercentage
    {
        get { return (currentPopulation / maxPopulation) * 100f; }
    }

    public bool CanSpawnFish()
    {
        return CurrentPopulationPercentage < lowPopulationThreshold;
    }

    public void DecreasePopulation(float amount)
    {
        currentPopulation = Mathf.Max(0, currentPopulation - amount);
    }

    public void IncreasePopulation(float amount)
    {
        currentPopulation = Mathf.Min(maxPopulation, currentPopulation + amount);
    }

    public void ResetPopulation()
    {
        currentPopulation = 0f;
    }
}