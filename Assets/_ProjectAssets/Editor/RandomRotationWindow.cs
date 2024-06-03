using UnityEditor;
using UnityEngine;

public class RandomRotationWindow : EditorWindow
{
    [MenuItem("Tools/Randomize X Rotation")]
    public static void ShowWindow()
    {
        GetWindow<RandomRotationWindow>("Random Rotation");
    }

    private void OnGUI()
    {
        GUILayout.Label("Random Rotation", EditorStyles.boldLabel);

        if (GUILayout.Button("Randomize X Rotation"))
        {
            RandomizeXRotation();
        }
    }

    private void RandomizeXRotation()
    {
        Transform[] selectedTransforms = Selection.transforms;

        foreach (Transform selectedTransform in selectedTransforms)
        {
            Undo.RecordObject(selectedTransform, "Randomize X Rotation");

            Vector3 randomRotation = new Vector3(selectedTransform.rotation.eulerAngles.x,Random.Range(0f, 360f) , selectedTransform.rotation.eulerAngles.z);
            selectedTransform.rotation = Quaternion.Euler(randomRotation);
        }
    }
}
