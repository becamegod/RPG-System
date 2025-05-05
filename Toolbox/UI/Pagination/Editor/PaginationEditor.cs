#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pagination))]
public class PaginationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Pagination converter = (Pagination)target;
        if (GUILayout.Button("SetCellSize"))
            converter.SetCellSize();
    }
}
#endif