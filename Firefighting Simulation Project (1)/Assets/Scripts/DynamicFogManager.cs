using UnityEngine;
using System.Collections.Generic; // CRUCIAL: This line is required to use Lists!

public class DynamicFogManager : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;

    // List called activeFires.
    public List<Transform> activeFires = new List<Transform>();

    [Header("Fog Settings")]
    public float maxFogDensity = 0.3f;
    public float minFogDensity = 0.1f;
    public float maxDistance = 15f;

    [Header("Performance")]
    public float checkInterval = 0.2f;

    void Start()
    {
        // Ensure Global Fog is actually turned on in your Lighting settings
        RenderSettings.fog = true;

        // Start the repeating check
        InvokeRepeating(nameof(UpdateFogDensity), 0f, checkInterval);
    }

    void UpdateFogDensity()
    {
        // Check .Count instead of .Length for Lists
        if (activeFires.Count == 0 || playerCamera == null) return;

        // 1. Find the distance to the CLOSEST fire
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

        // 2. Map the distance to a 0.0 - 1.0 percentage
        float distanceFactor = 1f - Mathf.Clamp01(closestDistance / maxDistance);

        // 3. Smoothly adjust the global fog density based on that percentage
        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, distanceFactor);
    }
}