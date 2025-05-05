//using UnityEditor;
//using UnityEngine;

//namespace DialogueSystem.Editor
//{
//    [CustomPropertyDrawer(typeof(MultipleChoicesSentence.Choice))]
//    public class DialogueEditor : PropertyDrawer
//    {
//        private static readonly EditorButton[] buttons = new EditorButton[]
//        {
//            new EditorButton("Add sentence", AddSentence),
//        };
//        private int buttonGroupHeight;

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.PropertyField(position, property, label, true);
//            buttonGroupHeight = SerializeHelper.AddButtons(position, property, buttons);
//        }

//        private static void AddSentence(SerializedProperty property)
//        {
//            var choice = (MultipleChoicesSentence.Choice)property.GetObject();
//            choice.AddSubSentence();
//        }

//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            return EditorGUI.GetPropertyHeight(property) + buttonGroupHeight;
//        }
//    }
//}
