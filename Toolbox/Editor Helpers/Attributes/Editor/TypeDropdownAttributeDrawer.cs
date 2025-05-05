using System;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
public class TypeDropdownAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // get details
        var atb = attribute as TypeDropdownAttribute;
        var typeNames = atb.typeNames;
        var fullTypeNames = atb.fullTypeNames;
        int selectedIndex = Array.IndexOf(fullTypeNames, property.stringValue);

        // dropdown
        int newIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, typeNames);

        // apply new value
        if (newIndex != selectedIndex && newIndex >= 0)
        {
            property.stringValue = fullTypeNames[newIndex];
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
}