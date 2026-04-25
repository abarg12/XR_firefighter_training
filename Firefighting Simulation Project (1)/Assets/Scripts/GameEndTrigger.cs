using UnityEngine;
using UnityEngine.UI;

public class GameEndTrigger : MonoBehaviour
{
    [SerializeField] private Button quitButton;

    private bool hasTriggered = false;

    private void Awake()
    {
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitApplication);
            quitButton.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (quitButton != null)
            quitButton.onClick.RemoveListener(QuitApplication);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered || !other.CompareTag("Player")) return;
        hasTriggered = true;

        NotificationManager.Show("trainingComplete", "Training Complete! Click OK to exit.");

        if (quitButton != null)
            quitButton.gameObject.SetActive(true);
    }

    private void QuitApplication()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}