using System.Collections;
using System.Linq;
using System.Reflection;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(NewButtonAttribute))]
[CustomPropertyDrawer(typeof(INewButton), true)]
public class NewButtonEditor : PropertyDrawer
{
    private int buttonGroupHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
        if (HasSerializeField(property)) return;

        var list = property.GetList();
        var persistent = attribute is NewButtonAttribute atb && atb.persistent;
        if (property.GetObject() is not null && !persistent && list != null)
        {
            buttonGroupHeight = 0;
            return;
        }
        buttonGroupHeight = SerializeHelper.AddButton(position, property, new(New));
    }

    private static bool HasSerializeField(SerializedProperty property)
    {
        // Get the field info
        var targetObject = property.serializedObject.targetObject;
        var targetType = targetObject.GetType();
        var fieldInfo = targetType.GetField(property.propertyPath, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        // Check if the field has [SerializeField] attribute
        return fieldInfo?.GetCustomAttribute<SerializeField>() != null;
    }

    private void New(SerializedProperty property)
    {
        var list = property.GetList();
        if (list != null)
        {
            var type = list.GetType().GetGenericArguments().Single();
            EditorHelper.ShowTypesMenu(type, false, (instance) =>
            {
                var list = property.GetList();
                var index = property.GetListIndex();
                list[index] = instance;
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            });
        }
        else
        {
            var type = fieldInfo.FieldType;
            EditorHelper.ShowTypesMenu(type, false, (instance) =>
            {
                property.serializedObject.Update();
                property.managedReferenceValue = instance;
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            });
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property) + buttonGroupHeight;
}
