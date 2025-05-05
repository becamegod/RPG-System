using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulkAnimClipSettings : EditorWindow
{

    ClipSettings[] clipSettings = new ClipSettings[255];
    ModelImporter lastSelected;

    [MenuItem("Imphenzia/BulkAnimClipSettings")]
    static void Init()
    {
        BulkAnimClipSettings window = ScriptableObject.CreateInstance<BulkAnimClipSettings>();
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.Show();
    }

    [Serializable]
    struct ClipSettings
    {
        public bool loopTime;
        public bool keepOriginalOrientation;
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    void OnGUI()
    {
        var obj = Selection.activeObject;
        if (obj == null)
        {
            return;
        }
        ModelImporter model = AssetImporter.GetAtPath((AssetDatabase.GetAssetPath(obj.GetInstanceID()))) as ModelImporter;

        if (model == null)
        {
            lastSelected = null;
            return;
        }

        var clips = model.clipAnimations;
        if (clips.Length == 0)
        {
            clips = model.defaultClipAnimations;
        }

        if (lastSelected != model)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].loopTime = clips[i].loopTime;
                clipSettings[i].keepOriginalOrientation = clips[i].keepOriginalOrientation;
            }
            lastSelected = model;
        }
        EditorGUILayout.HelpBox("EXTREME BETA - USE WITH CAUTION!", MessageType.Warning);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Bulk Actions", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Check All"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].loopTime = true;
            }
        }
        if (GUILayout.Button("Check None"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].loopTime = false;
            }
        }
        if (GUILayout.Button("Invert All"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].loopTime = !clipSettings[i].loopTime;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("Check All"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].keepOriginalOrientation = true;
            }
        }
        if (GUILayout.Button("Check None"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].keepOriginalOrientation = false;
            }
        }
        if (GUILayout.Button("Invert All"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clipSettings[i].keepOriginalOrientation = !clipSettings[i].loopTime;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("CLIP NAME", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("LOOP TIME", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();


        EditorGUI.BeginChangeCheck();
        for (int i = 0; i < clips.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(clips[i].name);
            clipSettings[i].loopTime = EditorGUILayout.Toggle(clipSettings[i].loopTime);
            clipSettings[i].keepOriginalOrientation = EditorGUILayout.Toggle(clipSettings[i].keepOriginalOrientation);
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Change Animation Settings"))
        {
            for (int i = 0; i < clips.Length; i++)
            {
                clips[i].loopTime = clipSettings[i].loopTime;
                clips[i].keepOriginalOrientation = clipSettings[i].keepOriginalOrientation;
            }
            model.clipAnimations = clips;
            model.SaveAndReimport();
        }
    }
}
