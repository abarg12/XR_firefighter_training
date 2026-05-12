using UnityEngine;
using TMPro;

public class WorldNotification : MonoBehaviour
{
    [SerializeField] private string notificationId;
    [SerializeField] private TextMeshProUGUI messageText;

    public string Id => notificationId;

    void Awake()
    {
        gameObject.SetActive(false);
        NotificationManager.Register(this);
    }

    void OnDestroy()
    {
        NotificationManager.Unregister(this);
    }

    public void Show(string message = null)
    {
        if (messageText != null && !string.IsNullOrEmpty(message))
            messageText.text = message;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}