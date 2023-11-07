using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ExtendedButton))]
public class ExtendedButtonEditor : ButtonEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space(); 

        ExtendedButton button = (ExtendedButton)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onPointerUp"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
