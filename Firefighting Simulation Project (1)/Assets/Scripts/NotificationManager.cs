using UnityEngine;
using System.Collections.Generic;

public static class NotificationManager
{
    private static readonly Dictionary<string, WorldNotification> registry =
        new Dictionary<string, WorldNotification>();

    public static void Register(WorldNotification notif)
    {
        if (notif == null || string.IsNullOrEmpty(notif.Id))
        {
            Debug.LogWarning($"WorldNotification is missing an ID: {notif?.name}");
            return;
        }
        registry[notif.Id] = notif;
    }

    public static void Unregister(WorldNotification notif)
    {
        if (notif == null) return;
        if (registry.TryGetValue(notif.Id, out var existing) && existing == notif)
            registry.Remove(notif.Id);
    }

    public static void Show(string id, string message = null)
    {
        if (registry.TryGetValue(id, out var notif) && notif != null)
            notif.Show(message);
        else
            Debug.LogWarning($"No WorldNotification registered with id '{id}'.");
    }

    public static void Hide(string id)
    {
        if (registry.TryGetValue(id, out var notif) && notif != null)
            notif.Hide();
    }
}