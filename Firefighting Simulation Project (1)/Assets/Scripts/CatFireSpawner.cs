using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class CatFireSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay = 0.5f;

    private GameObject endGameZone; 
    private GameObject firePrefab;
    private Transform[] fireSpawnPoints;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable != null)
            interactable.selectEntered.AddListener(OnSelected);
        else
            Debug.LogWarning("CatFireSpawner: no XRBaseInteractable found on this GameObject.");
    }

    void OnDestroy()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnSelected);
    }

    // dynamically set values
    public void Configure(GameObject firePrefab, Transform[] spawnPoints, GameObject endZone)
    {
        this.firePrefab = firePrefab;
        this.fireSpawnPoints = spawnPoints;
        this.endGameZone = endZone;
    }

    private void OnSelected(SelectEnterEventArgs args)
    {
        NotificationManager.Show("catRetrieved", "Cat retrieved.");
        NotificationManager.Show("catRetrieved2", "New objective: Find a way out of the building.");

        if (endGameZone != null)
        {
            endGameZone.SetActive(true);
        }

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends) r.enabled = false;

        Invoke(nameof(DoSpawn), spawnDelay);
    }

    private void DoSpawn()
    {
        if (firePrefab == null || fireSpawnPoints == null || fireSpawnPoints.Length == 0)
        {
            Debug.LogWarning("CatFireSpawner: not configured.");
        }
        else
        {
            foreach (Transform point in fireSpawnPoints)
            {
                if (point != null) {
                    GameObject spawnedFire = Instantiate(firePrefab, point.position, point.rotation);
                
                    FireSpreadNode fireNode = spawnedFire.GetComponent<FireSpreadNode>();
                    if (fireNode != null)
                    {
                        fireNode.Ignite();
                    }
                }
            }
        }

        gameObject.SetActive(false);
    }
}