using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    private static readonly int On = Animator.StringToHash("On");
    [SerializeField] private Light2D lightLamp;
    [SerializeField] private DayNight dayNightCycle;
    [SerializeField] Animator _animator;


    private void Start()
    {
        lightLamp.enabled = false;
        dayNightCycle.onCycleChanged.AddListener(state =>
        {
            ToggleLamp(state != CycleState.Day);
        });
    }

    private void ToggleLamp(bool state)
    {
        lightLamp.enabled = state;
        _animator.SetBool(On, state);
    }
}
