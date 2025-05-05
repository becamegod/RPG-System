using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VisibilityParent), true)]
public class VisibilityParentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var concreteTarget = target as VisibilityParent;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Show")) concreteTarget.Show();
        if (GUILayout.Button("Hide")) concreteTarget.Hide();
        GUILayout.EndHorizontal();
    }
}