using System.Collections;
using UnityEngine;

public class FireSpreadNode: MonoBehaviour
{
    [Header("Status")]
    public bool isOnFire = false;

    [Header("Spread Settings")]
    public float spreadRadius = 2.2f;
    public float minSpreadDelay = 10f;  // The minimum time before spreading
    public float maxSpreadDelay = 13f; // The maximum time before spreading

    [Header("Layer Masks")]
    public LayerMask fireNodeLayer;
    public LayerMask wallLayer;

    [Header("Visuals")]
    public GameObject fireVFX;

    void Start()
    {
        // Set initial visual state based on the Inspector checkbox
        if (isOnFire)
        {
            // Tell the fog manager we exist right at the start!
            FindObjectOfType<DynamicFogManager>().activeFires.Add(this.transform);

            fireVFX.SetActive(true);
            StartCoroutine(SpreadRoutine());
        }
        else
        {
            fireVFX.SetActive(false);
        }
    }

    public void Ignite()
    {
        // Prevent double-igniting
        if (isOnFire) return;

        isOnFire = true;
        FindObjectOfType<DynamicFogManager>().activeFires.Add(this.transform);
        fireVFX.SetActive(true);
        StartCoroutine(SpreadRoutine());
    }

    IEnumerator SpreadRoutine()
    {
        // Add a slight random delay at the very beginning.
        // This prevents a chain reaction of hundreds of nodes trying to 
        // calculate physics on the exact same frame, which causes VR stutter.
        yield return new WaitForSeconds(Random.Range(0f, 4f));

        while (isOnFire)
        {
            // Calculate a random delay between min spread and max spread seconds for this specific tick
            float currentSpreadDelay = Random.Range(minSpreadDelay, maxSpreadDelay);

            // Wait for that random delay before trying to spread
            yield return new WaitForSeconds(currentSpreadDelay);

            // Find all nodes within radius
            Collider[] neighbors = Physics.OverlapSphere(transform.position, spreadRadius, fireNodeLayer);

            foreach (Collider col in neighbors)
            {
                FireSpreadNode neighbor = col.GetComponent<FireSpreadNode>();

                // If we found a valid, unignited neighbor
                if (neighbor != null && neighbor != this && !neighbor.isOnFire)
                {
                    // Check for walls
                    Vector3 directionToNeighbor = neighbor.transform.position - transform.position;
                    if (!Physics.Raycast(transform.position, directionToNeighbor, directionToNeighbor.magnitude, wallLayer))
                    {
                        // Ignite the neighbor
                        neighbor.Ignite();
                    }
                }
            }
        }
    }

    // Draws a helpful visual sphere in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}