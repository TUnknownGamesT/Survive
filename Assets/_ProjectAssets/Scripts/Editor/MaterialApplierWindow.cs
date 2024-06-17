using UnityEngine;
using UnityEditor;

public class MaterialApplierWindow : EditorWindow
{
    private Material selectedMaterial;
    private string materialName = "";

    [MenuItem("Custom/Material Applier Window")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MaterialApplierWindow), false, "Material Applier");
    }

    void OnGUI()
    {
        GUILayout.Label("Material Applier", EditorStyles.boldLabel);

        // Material selection field
        selectedMaterial = EditorGUILayout.ObjectField("Material", selectedMaterial, typeof(Material), true) as Material;

        // Material name input field
        materialName = EditorGUILayout.TextField("Material Name", materialName);

        // Apply Material button
        if (GUILayout.Button("Apply Material"))
        {
            ApplyMaterialToSelectedObjects();
        }
    }

    void ApplyMaterialToSelectedObjects()
    {
        if (selectedMaterial != null)
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            foreach (GameObject obj in selectedObjects)
            {
                Renderer renderer = obj.GetComponent<Renderer>();

                if (renderer != null)
                {
                    Material[] materials = renderer.sharedMaterials;

                    for (int i = 0; i < materials.Length; i++)
                    {
                        materials[i] = selectedMaterial;
                    }

                    renderer.sharedMaterials = materials;
                    Debug.Log($"Material {selectedMaterial.name} applied to {obj.name}.");
                }
                else
                {
                    Debug.LogError($"Object {obj.name} does not have a Renderer component.");
                }
            }
        }
        else
        {
            Debug.LogError("Please select a material to apply.");
        }
    }
}