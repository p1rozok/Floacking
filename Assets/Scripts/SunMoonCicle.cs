using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class SunMoonCicle : MonoBehaviour
{

    [SerializeField] private GameObject moon;
    [SerializeField] private GameObject sun;
    [SerializeField] private DayNight dayNightCycle;

    private void Update()
    {
        moon.transform.rotation=Quaternion.identity;
        sun.transform.rotation=Quaternion.identity;
        
        var mappedRotation = Utils.Map(dayNightCycle.NormalizedTime, 0f, 1f, 0f, 360f);

        transform.rotation = Quaternion.Euler(0f, 0f, mappedRotation + 90f);
    }
}
