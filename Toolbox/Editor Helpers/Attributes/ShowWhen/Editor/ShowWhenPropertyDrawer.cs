#if UNITY_EDITOR
using System;
using System.Linq;
using System.Text;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(ShowWhenAttribute))]
public class HideWhenPropertyDrawer : PropertyDrawer
{
    ShowWhenAttribute Atb => attribute as ShowWhenAttribute;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool enabled = Evaluate(Atb, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (!Atb.HideInInspector || enabled) EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        bool enabled = Evaluate(Atb, property);

        if (!Atb.HideInInspector || enabled) return EditorGUI.GetPropertyHeight(property, label);
        else return -EditorGUIUtility.standardVerticalSpacing;
    }

    private bool Evaluate(ShowWhenAttribute attribute, SerializedProperty property)
    {
        var conditionField = attribute.ConditionField.Replace(" ", "");
        return Evaluate(conditionField, property);
    }

    private bool Evaluate(string expression, SerializedProperty property)
    {
        // recurse
        if (expression.Contains("&&"))
        {
            var expressions = expression.Split("&&");
            return expressions.All(expression => Evaluate(expression, property));
        }
        else if (expression.Contains("||"))
        {
            var subExpressions = expression.Split("||");
            return subExpressions.Any(expression => Evaluate(expression, property));
        }

        // negation check
        var conditionFieldBuilder = new StringBuilder(expression);
        var negation = false;
        if (conditionFieldBuilder[0] == '!')
        {
            negation = true;
            conditionFieldBuilder = conditionFieldBuilder.Remove(0, 1);
        }

        var conditionPath = property.propertyPath.Replace(property.name, conditionFieldBuilder.ToString());
        var conditionValue = property.serializedObject.FindProperty(conditionPath);

        if (conditionValue != null)
        {
            // evaluate
            var evaluation = false;
            if (Atb.Value is Enum) evaluation = (int)Atb.Value == conditionValue.enumValueIndex;
            else evaluation = conditionValue.boolValue;

            // negate
            if (negation) evaluation = !evaluation;

            return evaluation;
        }

        Debug.LogWarning("Attempting to use a HideWhenAttribute but no matching SourcePropertyValue found in object: " + expression);
        return true;
    }
}
#endif