using UnityEngine;
using UnityEditor;

public class ShaderChanger : EditorWindow
{
    [MenuItem("Tools/Change All Shaders")]
    public static void ChangeShaders()
    {
        // Change this string to the exact name of the shader you want
        Shader newShader = Shader.Find("Universal Render Pipeline/Simple Lit");

        string[] guids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat != null)
            {
                mat.shader = newShader;
            }
        }
        Debug.Log("All materials updated!");
    }
}