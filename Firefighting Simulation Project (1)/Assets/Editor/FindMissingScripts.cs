using UnityEditor;
using UnityEngine;
using System.IO;

public class FindMissingScripts
{
    [MenuItem("Tools/Find Missing Scripts in Scene")]
    static void FindInScene()
    {
        var allObjects = GameObject.FindObjectsOfType<GameObject>(includeInactive: true);
        int found = 0;
        foreach (var go in allObjects)
        {
            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Debug.LogWarning($"Missing script on scene GameObject: {GetPath(go)}", go);
                    found++;
                }
            }
        }
        Debug.Log($"Scene scan done. Found {found} missing script reference(s).");
    }

    [MenuItem("Tools/Find Missing Scripts in Prefabs")]
    static void FindInPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int found = 0;
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            var allChildren = prefab.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var t in allChildren)
            {
                var components = t.GetComponents<Component>();
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        Debug.LogWarning($"Missing script in prefab: {path} → {GetPath(t.gameObject)}", prefab);
                        found++;
                    }
                }
            }
        }
        Debug.Log($"Prefab scan done. Found {found} missing script reference(s).");
    }

    static string GetPath(GameObject go)
    {
        string path = go.name;
        Transform t = go.transform.parent;
        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }
        return path;
    }
}