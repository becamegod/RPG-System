using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(AutoLabelAttribute))]
public class AutoLabeledEditor : PropertyDrawer
{
    const string Key = "showAutoLabel";
    const string MenuPath = "Tools/" + nameof(ToggleDisplayLabel);

    static int Show => PlayerPrefs.GetInt(Key, 1);

    [MenuItem(MenuPath)]
    public static void ToggleDisplayLabel()
    {
        var show = Show;
        show = 1 - show;
        PlayerPrefs.SetInt(Key, show);
    }

    [MenuItem(MenuPath, true)]
    public static bool ValidateToggleDisplayLabel()
    {
        Menu.SetChecked(MenuPath, Show.ToBool());
        return true;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (Show.ToBool()) EditorGUI.PropertyField(position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (Show.ToBool()) return EditorGUI.GetPropertyHeight(property);
        return 0;
    }
}