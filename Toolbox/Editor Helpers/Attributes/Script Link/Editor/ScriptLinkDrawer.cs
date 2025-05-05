using System;

using UnityEditor;

using UnityEngine;

namespace SkillSystem
{
    [CustomPropertyDrawer(typeof(ScriptLinkAttribute), true)]
    public class ScriptLinkDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // draw default field if disabled
            if (!ScriptLinkSettings.Instance.EnableLink)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            // get type
            var type = property.GetParent()?.GetType(); // or GetObject for not-attribute
            if (type == null) return;

            // draw label
            var labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);

            // draw button
            var buttonRect = new Rect(position.x + labelWidth, position.y, position.width - labelWidth, position.height);
            EditorGUI.BeginDisabledGroup(true);
            GUI.Button(buttonRect, property.stringValue, EditorStyles.objectField);
            EditorGUI.EndDisabledGroup();

            // handle clicking
            var ev = Event.current;
            if (ev.type == EventType.MouseDown && buttonRect.Contains(ev.mousePosition))
            {
                if (ev.clickCount == 1 && ScriptLinkSettings.Instance.PingScript) PingScript(type);
                else if (ev.clickCount == 2) OpenScript(type);
                ev.Use(); // Consume the event
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property);

        private void PingScript(Type type)
        {
            if (!ScriptLinkCache.TryGetScriptPath(type.Name, out var path)) return;
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            EditorGUIUtility.PingObject(asset);
        }

        private void OpenScript(Type type)
        {
            if (ScriptLinkCache.TryGetScriptPath(type.Name, out string path)) AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<MonoScript>(path));
            else Debug.LogError($"Script file for {type.Name} not found in cache.");
        }
    }
}