using System;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace SkillSystem
{
    [CustomPropertyDrawer(typeof(EffectIdDropdownAttribute))]
    public class EffectIdDropdownDrawer : PropertyDrawer
    {
        private static string[] effectIds;

        static EffectIdDropdownDrawer()
        {
            // Find all effect classes with EffectIdAttribute
            var type = typeof(SimpleEffect);
            effectIds = type.Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(type))
                .Select(t => t.GetCustomAttribute<EffectIdAttribute>()?.id)
                .Where(id => id != null)
                .ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                int selectedIndex = Array.IndexOf(effectIds, property.stringValue);
                selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, effectIds);
                if (selectedIndex >= 0) property.stringValue = effectIds[selectedIndex];
            }
            else EditorGUI.PropertyField(position, property, label);
        }
    }


    [CustomPropertyDrawer(typeof(ChangeCheckAttribute))]
    public class ChangeCheckDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(position, property, label);
            if (EditorGUI.EndChangeCheck())
            {
                var target = property.serializedObject.targetObject;
                if (EditorUtility.IsPersistent(target))
                {
                    EditorUtility.SetDirty(target);
                    AssetDatabase.SaveAssetIfDirty(target);
                }
                var path = AssetDatabase.GetAssetPath(target);
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Debug.Log($"Changed: {target}");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property, label);
    }
}