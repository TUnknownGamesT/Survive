using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : EditorWindow
{
    private GameObject prefabToReplaceWith;

    [MenuItem("Custom/Replace With Prefab")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ReplaceWithPrefab), false, "Replace With Prefab");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace With Prefab", EditorStyles.boldLabel);

        // Prefab selection field
        prefabToReplaceWith = EditorGUILayout.ObjectField("Prefab", prefabToReplaceWith, typeof(GameObject), true) as GameObject;

        // Replace With Prefab button
        if (GUILayout.Button("Replace With Prefab"))
        {
            ReplaceSelectedObjectsWithPrefab();
        }
    }

    void ReplaceSelectedObjectsWithPrefab()
    {
        if (prefabToReplaceWith != null)
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            foreach (GameObject obj in selectedObjects)
            {
                // Instantiate the prefab
                GameObject newObject = PrefabUtility.InstantiatePrefab(prefabToReplaceWith) as GameObject;

                // Copy transform values
                newObject.transform.position = obj.transform.position;
                newObject.transform.rotation = obj.transform.rotation;
                newObject.transform.localScale = obj.transform.localScale;

                // Destroy the original object
                DestroyImmediate(obj);
            }

            Debug.Log($"Selected objects replaced with {prefabToReplaceWith.name}.");
        }
        else
        {
            Debug.LogError("Please select a prefab to replace with.");
        }
    }
}