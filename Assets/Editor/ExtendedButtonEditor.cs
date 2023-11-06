using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ExtendedButton))]
public class ExtendedButtonEditor : ButtonEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI(); // Draw the default inspector

        EditorGUILayout.Space(); // Add a space in the inspector

        // Draw the onPointerUp event field
        ExtendedButton button = (ExtendedButton)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onPointerUp"), true);

        serializedObject.ApplyModifiedProperties(); // Apply properties to allow undo
    }
}
