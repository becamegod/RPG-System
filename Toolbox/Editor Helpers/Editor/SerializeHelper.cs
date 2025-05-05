using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using UnityEditor;

using UnityEngine;

public static class SerializeHelper
{
    public static object GetParent(this SerializedProperty prop) => GetObject(prop, -1);

    public static IList GetList(this SerializedProperty prop) => GetObject(prop, 0, true) as IList;

    public static object GetObject(this SerializedProperty prop, int depthOffset = 0, bool getList = false)
    {
        var path = prop.propertyPath.Replace(".Array.data[", "[");
        object obj = prop.serializedObject.targetObject;
        var elements = path.Split('.');
        foreach (var element in elements.Take(elements.Length + depthOffset))
        {
            if (element.Contains("["))
            {
                var elementName = element[..element.IndexOf("[")];
                var index = Convert.ToInt32(element[element.IndexOf("[")..].Replace("[", "").Replace("]", ""));

                if (getList && element == elements.Last()) obj = GetValue(obj, elementName);
                else obj = GetArrayElementValue(obj, elementName, index);
            }
            else obj = GetValue(obj, element);
        }
        return obj;
    }

    public static int GetListIndex(this SerializedProperty prop)
    {
        var path = prop.propertyPath;
        return Convert.ToInt16(path[(path.LastIndexOf("[") + 1)..path.LastIndexOf("]")]);
    }

    [Obsolete("We can just use PropertyDrawer.fieldInfo.FieldType")]
    public static Type GetFieldType(this SerializedProperty prop)
    {
        var source = prop.GetParent();

        var name = prop.propertyPath.Replace(".Array.data[", "[");
        name = name[(name.LastIndexOf('.') + 1)..];

        var member = GetMember(source, name);
        if (member is FieldInfo field) return field.FieldType;
        else if (member is PropertyInfo property) return property.PropertyType;
        return null;
    }

    public static object GetArrayElementValue(object source, string name, int index)
    {
        var enumerable = GetValue(source, name) as IEnumerable;
        var enm = enumerable.GetEnumerator();
        while (index-- >= 0) enm.MoveNext();
        return enm.Current;
    }

    public static object GetValue(object source, string name)
    {
        var member = GetMember(source, name);
        if (member is FieldInfo field) return field.GetValue(source);
        else if (member is PropertyInfo property) return property.GetValue(source);
        return null;
    }

    public static MemberInfo GetMember(object source, string name)
    {
        if (source == null) return null;
        var type = source.GetType();

        // field
        var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (field != null) return field;

        // property
        return type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
    }

    public static int AddButton(Rect position, SerializedProperty property, EditorButton button, int buttonHeight = 18)
    {
        return AddButtons(position, property, new EditorButton[] { button }, buttonHeight);
    }

    public static int AddButtons(Rect position, SerializedProperty property, EditorButton[] buttons, int buttonHeight = 18)
    {
        if (!property.isExpanded && property.GetObject() != null) return 0;

        position.y += EditorGUI.GetPropertyHeight(property);
        position.height = buttonHeight;
        foreach (var button in buttons)
        {
            if (GUI.Button(position, button.label)) button.onClick.Invoke(property);
            position.y += buttonHeight;
        }
        return buttonHeight * buttons.Length;
    }
}

public readonly struct EditorButton
{
    public readonly string label;
    public readonly Action<SerializedProperty> onClick;

    public EditorButton(Action<SerializedProperty> onClick)
    {
        this.onClick = onClick;
        label = onClick.Method.Name.SplitPascalCase();
    }

    public EditorButton(Action<SerializedProperty> onClick, string label) : this(onClick)
    {
        this.label = label;
    }
}