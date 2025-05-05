using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace WeaponSystem
{
    [CustomPropertyDrawer(typeof(ComboAttack))]
    public class ComboAttackEditor : PropertyDrawer
    {
        private int buttonGroupHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            if (property.isExpanded) buttonGroupHeight = SerializeHelper.AddButtons(position, property, new EditorButton[]
            {
                new(AddAction),
                new(AddConnectedAttack)
            });
            else buttonGroupHeight = 0;
        }

        private static void AddAction(SerializedProperty property)
        {
            var attack = property.GetObject() as ComboAttack;
            attack.AddAction();
        }

        private static void AddConnectedAttack(SerializedProperty property)
        {
            var attack = property.GetObject() as ComboAttack;
            attack.AddConnectedAttack();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUI.GetPropertyHeight(property) + buttonGroupHeight;
    }

    [CustomPropertyDrawer(typeof(ComboTree))]
    public class ComboTreeEditor : PropertyDrawer
    {
        private int buttonGroupHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            buttonGroupHeight = SerializeHelper.AddButtons(position, property, new EditorButton[]
            {
                new(AddComboAttack)
            });
        }

        private static void AddComboAttack(SerializedProperty property)
        {
            var tree = (ComboTree)property.GetObject();
            tree.AddComboAttack();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + buttonGroupHeight;
        }
    }
}
