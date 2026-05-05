using System.Collections;
using UnityEngine;

public class FireSpreadNode: MonoBehaviour
{
    [Header("Status")]
    public bool isOnFire = false;

    [Header("Spread Settings")]
    public float spreadRadius = 2.2f;
    public float minSpreadDelay = 10f;  // min time before spreading
    public float maxSpreadDelay = 13f; // max time before spreading

    [Header("Layer Masks")]
    public LayerMask fireNodeLayer;
    public LayerMask wallLayer;

    [Header("Visuals")]
    public GameObject fireVFX;

    void Start()
    {
        if (isOnFire)
        {
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
        if (isOnFire) return;

        isOnFire = true;
        
        DynamicFogManager fogManager = FindObjectOfType<DynamicFogManager>();
        if (fogManager != null)
        {
            fogManager.activeFires.Add(this.transform);
        }

        if (fireVFX != null)
        {
            fireVFX.SetActive(true);
            
            ParticleSystem[] allParticles = fireVFX.GetComponentsInChildren<ParticleSystem>();
            foreach(ParticleSystem ps in allParticles)
            {
                ps.Play();
            }
        }

        StartCoroutine(SpreadRoutine());
    }

    IEnumerator SpreadRoutine()
    {
        yield return new WaitForSeconds(Random.Range(0f, 4f));

        while (isOnFire)
        {
            float currentSpreadDelay = Random.Range(minSpreadDelay, maxSpreadDelay);
            yield return new WaitForSeconds(currentSpreadDelay);
            Collider[] neighbors = Physics.OverlapSphere(transform.position, spreadRadius, fireNodeLayer);

            foreach (Collider col in neighbors)
            {
                FireSpreadNode neighbor = col.GetComponent<FireSpreadNode>();

                if (neighbor != null && neighbor != this && !neighbor.isOnFire)
                {
                    Vector3 directionToNeighbor = neighbor.transform.position - transform.position;
                    if (!Physics.Raycast(transform.position, directionToNeighbor, directionToNeighbor.magnitude, wallLayer))
                    {
                        neighbor.Ignite();
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}