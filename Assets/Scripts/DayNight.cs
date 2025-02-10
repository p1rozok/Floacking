using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public enum CycleState
{
    Day,
    Night,
}
public class DayNight : MonoBehaviour
{
    [SerializeField] private float cycleDuration;
    [SerializeField] private Gradient dayNightGradient;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Light2D sun;
   
    private CycleState _cycleState = CycleState.Day;
    private float _sunIntensity;

    public float currentCycleTime = 0f;
    public float currentCycleNormalizedTime = 0f;
    public UnityEvent<CycleState> onCycleChanged = new UnityEvent<CycleState>();

    public float NormalizedTime => currentCycleNormalizedTime;

    private void Update()
    {
        HandleCycleTime();
        NormalizeCycleTime();
        ChangeColorByTime();
        ChangeSunIntensityByTime();
        HandleCycleEvents();
    }

    private void HandleCycleTime()
    {
        if (currentCycleTime >= cycleDuration)
        {
            currentCycleTime = 0f;
        }
        else
        {
            currentCycleTime += Time.deltaTime;
        }
    }

    private void NormalizeCycleTime()
    {
        currentCycleNormalizedTime = Mathf.Clamp(currentCycleTime / cycleDuration, 0, cycleDuration);
    }

    private void ChangeColorByTime()
    {
        background.color = dayNightGradient.Evaluate(currentCycleNormalizedTime);
    }

    private void ChangeSunIntensityByTime()
    {
        if (currentCycleNormalizedTime <= 0.5f)
        {
             _sunIntensity = Utils.Map(currentCycleNormalizedTime, 0f, 0.5f, 1f, 0f);
        }
        if (currentCycleNormalizedTime >= 0.5f)
        {
            _sunIntensity = Utils.Map(currentCycleNormalizedTime, 0.5f, 1f, 0f, 1f);
        }
        
        sun.intensity = _sunIntensity;
    }

    private void HandleCycleEvents()
    {
        var oldState = _cycleState;
        
        if (currentCycleNormalizedTime <= 0.4f)
        {
            _cycleState = CycleState.Day;
        }

        if (currentCycleNormalizedTime is >= 0.4f and <= 0.6f)
        {
            _cycleState = CycleState.Night;
        }

        if (currentCycleNormalizedTime >= 0.6f)
        {
            _cycleState = CycleState.Day;
        }

        if (oldState != _cycleState)
        {
            onCycleChanged.Invoke(_cycleState);
        }
    }
}
