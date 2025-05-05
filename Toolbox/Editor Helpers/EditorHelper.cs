#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.Collections.Generic;

using UnityEngine;

using System;
using System.Linq;
using System.Reflection;

public static class EditorHelper
{
    public static void ShowTypesMenuToAdd<T>(List<T> list, bool includeBaseClass = false, Action<T> onSelected = null)
    {
#if UNITY_EDITOR
        var types = Helper.GetAllTypesDerivedFrom<T>();
        if (includeBaseClass) types = Enumerable.Concat(new[] { typeof(T) }, types);
        GenericMenu menu = new();
        foreach (var type in types)
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                T instance = (T)Activator.CreateInstance(type);
                list ??= new();
                list.Add(instance);
                onSelected?.Invoke(instance);
            });
        menu.ShowAsContext();
#endif
    }

    public static void ShowTypesMenu<T>(bool includeBaseClass = false, Action<T> onSelected = null)
    {
#if UNITY_EDITOR
        ShowTypesMenu(typeof(T), includeBaseClass, (object instance) => onSelected?.Invoke((T)instance));
#endif
    }

    public static void ShowTypesMenu(Type type, bool includeBaseClass = false, Action<object> onSelected = null)
    {
#if UNITY_EDITOR
        var types = Helper.GetAllTypesDerivedFrom(type);
        if (includeBaseClass) types = Enumerable.Concat(new[] { type }, types);
        GenericMenu menu = new();
        foreach (var t in types)
            menu.AddItem(new GUIContent(t.Name), false, () =>
            {
                var instance = Activator.CreateInstance(t);
                onSelected?.Invoke(instance);
            });
        menu.ShowAsContext();
#endif
    }

    //[Obsolete]
    //public static bool IsExpanded(SerializedProperty property) => InternalEditorUtility.GetIsInspectorExpanded(property.serializedObject.targetObject);

    // Origin: https://discussions.unity.com/t/query-whether-inspector-is-folded/118298/2
    //private static Type m_InspectorWindowType = null;
    //private static MethodInfo m_GetAllInspectors = null;
    //private static MethodInfo m_GetTracker = null;
    //static EditorHelper()
    //{
    //    foreach (var T in typeof(EditorApplication).Assembly.GetTypes())
    //    {
    //        if (T.Name == "InspectorWindow")
    //        {
    //            m_InspectorWindowType = T;
    //            m_GetAllInspectors = T.GetMethod("GetAllInspectorWindows", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
    //            m_GetTracker = T.GetMethod("GetTracker", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
    //            break;
    //        }
    //    }
    //}

    //public static EditorWindow[] GetAllInspectorWindows()
    //{
    //    var array = m_GetAllInspectors.Invoke(null, null) as EditorWindow[];
    //    return array;
    //}

    //public static ActiveEditorTracker GetTracker(EditorWindow aWin)
    //{
    //    var tracker = m_GetTracker.Invoke(aWin, null) as ActiveEditorTracker;
    //    return tracker;
    //}

    //public static void EnforceExpanded(Type aCustomEditor)
    //{
    //    var windows = GetAllInspectorWindows();
    //    for (int n = 0; n < windows.Length; n++)
    //    {
    //        var tracker = GetTracker(windows[n]);
    //        var editors = tracker.activeEditors;
    //        for (int i = 0; i < editors.Length; i++)
    //        {
    //            if (editors.GetType() == aCustomEditor)
    //            {
    //                int visible = tracker.GetVisible(i);
    //                if (visible == 0)
    //                {
    //                    tracker.SetVisible(i, 1);
    //                }
    //            }
    //        }
    //    }
    //}

    //    public static bool CheckFold()
    //    {
    //#if UNITY_EDITOR
    //        var _sceneHierarchyWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
    //        var _getExpandedIDs = _sceneHierarchyWindowType.GetMethod("GetExpandedIDs", BindingFlags.NonPublic | BindingFlags.Instance);
    //        var _lastInteractedHierarchyWindow ??= _sceneHierarchyWindowType.GetProperty("lastInteractedHierarchyWindow", BindingFlags.Public | BindingFlags.Static);
    //        if (_lastInteractedHierarchyWindow == null)
    //        {
    //            return false;
    //        }
    //        var _expandedIDs = _getExpandedIDs.Invoke(_lastInteractedHierarchyWindow.GetValue(null), null) as int[];

    //        // Is expanded?
    //        return _expandedIDs.Contains(gameObject.GetInstanceID());
    //#endif
    //    }
}
