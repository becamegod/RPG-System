using System;
using System.Linq;

using UnityEditor;

using UnityEngine;

public class BulkAnimationClipSettings : EditorWindow
{
    private bool loop = true;
    private bool originalOrientation = true;
    private bool originalPositionY = true;
    private bool originalPositionXZ = true;
    private bool bakeRotation = true;
    private bool bakeHeight = true;
    private bool bakePosition;

    const string featureName = "Bulk Animation Clip Settings";

    [MenuItem("Tools/" + featureName)]
    [MenuItem("Assets/" + featureName)]
    private static void Init()
    {
        var window = CreateInstance<BulkAnimationClipSettings>();
        window.Show();
    }

    [MenuItem("Assets/" + featureName, true)]
    private static bool Validate()
    {
        try 
        {
            // selections
            var objects = Selection.objects;
            if (objects == null) return false;

            // models
            var models = objects.Select(obj => AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj.GetInstanceID())) as ModelImporter).Where(model => model != null);
            if (models.Count() == 0) return false;

            return true;
        }
        catch (Exception) {return false;}
    }

    private void OnSelectionChange() => Repaint();

    private void OnGUI()
    {
        // selections
        var objects = Selection.objects;
        if (objects == null) return;

        // models
        var models = objects.Select(obj => AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(obj.GetInstanceID())) as ModelImporter).Where(model => model != null);
        if (models.Count() == 0) return;

        // settings
        loop = EditorGUILayout.Toggle("Loop", loop);
        bakeRotation = EditorGUILayout.Toggle("Bake Rotation", bakeRotation);
        bakeHeight = EditorGUILayout.Toggle("Bake Height", bakeHeight);
        bakePosition = EditorGUILayout.Toggle("Bake Position", bakePosition);
        originalOrientation = EditorGUILayout.Toggle("Original Orientation", originalOrientation);
        originalPositionY = EditorGUILayout.Toggle("Original Position Y", originalPositionY);
        originalPositionXZ = EditorGUILayout.Toggle("Original Position XZ", originalPositionXZ);

        // Apply
        if (GUILayout.Button("Apply"))
        {
            foreach (var model in models)
            {
                var clips = model.clipAnimations;
                foreach (var clip in clips)
                {
                    clip.loopTime = loop;
                    clip.lockRootRotation = bakeRotation;
                    clip.lockRootHeightY = bakeHeight;
                    clip.lockRootPositionXZ = bakePosition;
                    clip.keepOriginalOrientation = originalOrientation;
                    clip.keepOriginalPositionY = originalPositionY;
                    clip.keepOriginalPositionXZ = originalPositionXZ;
                }
                model.clipAnimations = clips;
                model.SaveAndReimport();
            }
        }
    }
}
