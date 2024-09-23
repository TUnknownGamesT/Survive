using UnityEngine;
using UnityEditor;

public class ColliderSwapper : EditorWindow
{
    private GameObject targetObject;

    [MenuItem("Tools/Collider Swapper")]
    public static void ShowWindow()
    {
        GetWindow<ColliderSwapper>("Collider Swapper");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select Target Object", EditorStyles.boldLabel);

        targetObject = (GameObject)EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Swap MeshColliders with BoxColliders"))
        {
            if (targetObject != null)
            {
                SwapColliders();
            }
            else
            {
                Debug.LogWarning("Please select a GameObject.");
            }
        }
    }

    private void SwapColliders()
    {
        MeshCollider[] meshColliders = targetObject.GetComponentsInChildren<MeshCollider>();

        if (meshColliders.Length == 0)
        {
            Debug.LogWarning("No MeshColliders found in the selected object.");
            return;
        }

        Undo.RegisterCompleteObjectUndo(targetObject, "Swap MeshColliders");

        foreach (MeshCollider meshCollider in meshColliders)
        {
            GameObject obj = meshCollider.gameObject;

            // Create BoxCollider with approximate size
            BoxCollider boxCollider = Undo.AddComponent<BoxCollider>(obj);
            boxCollider.center = meshCollider.bounds.center - obj.transform.position;
            boxCollider.size = meshCollider.bounds.size;

            // Remove the MeshCollider
            Undo.DestroyObjectImmediate(meshCollider);
        }

        Debug.Log("MeshColliders swapped with BoxColliders.");
    }
}