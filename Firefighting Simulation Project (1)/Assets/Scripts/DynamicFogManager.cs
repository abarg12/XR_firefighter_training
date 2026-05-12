using UnityEngine;
using System.Collections.Generic;

public class DynamicFogManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;

    public List<Transform> activeFires = new List<Transform>();

    [Header("Fog Settings")]
    public float maxFogDensity = 0.3f;
    public float minFogDensity = 0.1f;
    public float maxDistance = 15f;

    [Header("Performance")]
    public float checkInterval = 0.2f;

    void Start()
    {
        // turns on global fog
        RenderSettings.fog = true;

        InvokeRepeating(nameof(UpdateFogDensity), 0f, checkInterval);
    }

    void UpdateFogDensity()
    {
        if (activeFires.Count == 0 || playerCamera == null) return;

        // get closest fire
        float closestDistance = Mathf.Infinity;
        foreach (Transform fire in activeFires)
        {
            if (fire != null)
            {
                float dist = Vector3.Distance(playerCamera.position, fire.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                }
            }
        }

        float distanceFactor = 1f - Mathf.Clamp01(closestDistance / maxDistance);

        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, distanceFactor);
    }
}