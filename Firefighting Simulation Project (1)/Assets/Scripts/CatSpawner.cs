using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [SerializeField] private GameObject catPrefab;
    [SerializeField] private Transform[] catSpawnPoints;

    [Header("Fire config (passed to cat)")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform[] fireSpawnPoints;

    [SerializeField] private GameObject endGameZone;

    [Header("Notification to reposition with cat")]
    [SerializeField] private Transform catNotificationCanvas;
    [SerializeField] private Vector3 notificationOffset = new Vector3(0f, 0.6f, 0f);

    [Header("Second Notification to reposition with cat")]
    [SerializeField] private Transform catNotificationCanvas2;
    [SerializeField] private Vector3 notificationOffset2 = new Vector3(0f, 0.5f, 0f);

    void Start()
    {
        if (catPrefab == null || catSpawnPoints.Length == 0) return;

        Transform chosen = catSpawnPoints[Random.Range(0, catSpawnPoints.Length)];
        GameObject cat = Instantiate(catPrefab, chosen.position, chosen.rotation);

        var fireSpawner = cat.GetComponent<CatFireSpawner>();
        if (fireSpawner != null)
            fireSpawner.Configure(firePrefab, fireSpawnPoints, endGameZone);

        if (catNotificationCanvas != null)
        {
            catNotificationCanvas.position = chosen.position + notificationOffset;
            catNotificationCanvas.rotation = chosen.rotation * Quaternion.Euler(0f, 180f, 0f);;
        }

        if (catNotificationCanvas2 != null)
        {
            catNotificationCanvas2.position = chosen.position + notificationOffset2;
            catNotificationCanvas2.rotation = chosen.rotation * Quaternion.Euler(0f, 180f, 0f);;
        }
    }
}