using System.Collections;
using UnityEngine;

public class BuildingFlicker : MonoBehaviour
{
    [Header("Interval Between Flickers")]
    [SerializeField] private float minInterval = 10f;
    [SerializeField] private float maxInterval = 15f;

    [Header("Flicker Burst")]
    [SerializeField] private float minBurstDuration = 0.3f;
    [SerializeField] private float maxBurstDuration = 1.2f;
    [SerializeField] private float minBlinkTime = 0.05f;
    [SerializeField] private float maxBlinkTime = 0.15f;

    [Header("Blackout")]
    [SerializeField] private float minBlackoutDuration = 0.5f;
    [SerializeField] private float maxBlackoutDuration = 1.5f;

    [Header("Behaviour")]
    [SerializeField] private bool flickerOnStart = true;

    private Light[] _lights;

    void Start()
    {
        _lights = GetComponentsInChildren<Light>(includeInactive: true);

        if (flickerOnStart)
            StartCoroutine(FlickerAllLights());
    }

    public void SetFlickering(bool active)
    {
        StopAllCoroutines();
        SetAllLights(true);

        if (active)
            StartCoroutine(FlickerAllLights());
    }

    private IEnumerator FlickerAllLights()
    {
        while (true)
        {
            SetAllLights(true);
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            float burstEnd = Time.time + Random.Range(minBurstDuration, maxBurstDuration);
            bool on = true;
            while (Time.time < burstEnd)
            {
                on = !on;
                SetAllLights(on);
                yield return new WaitForSeconds(Random.Range(minBlinkTime, maxBlinkTime));
            }

            SetAllLights(false);
            yield return new WaitForSeconds(Random.Range(minBlackoutDuration, maxBlackoutDuration));
        }
    }

    private void SetAllLights(bool enabled)
    {
        foreach (var l in _lights)
            l.enabled = enabled;
    }
}