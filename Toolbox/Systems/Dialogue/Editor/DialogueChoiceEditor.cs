using System.IO;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [CustomPropertyDrawer(typeof(MultipleChoicesSentence.Choice))]
    public class DialogueChoiceEditor : PropertyDrawer
    {
        private static readonly EditorButton[] buttons = new EditorButton[]
        {
            new EditorButton(AddDialogue),
            new EditorButton(AddSentence),
            //new EditorButton("Add dialogue inside", AddDialogueInside),
            //new EditorButton("Delete dialogue inside", DeleteDialogueInside),
        };
        private int buttonGroupHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
            buttonGroupHeight = SerializeHelper.AddButtons(position, property, buttons);
        }

        private static void AddSentence(SerializedProperty property)
        {
            var choice = (MultipleChoicesSentence.Choice)property.GetObject();
            if (!choice.Dialogue)
            {
                EditorUtility.DisplayDialog("Invalid operation", "The Dialogue field is null, you must create dialogue before adding sentence", "OK");
                return;
            }
            choice.AddSubSentence();
            //EditorUtility.SetDirty(property.serializedObject.targetObject);
        }

        private static void AddDialogue(SerializedProperty property)
        {
            // init 
            var multiChoiceSentence = (MultipleChoicesSentence)property.GetParent();
            var choice = (MultipleChoicesSentence.Choice)property.GetObject();
            var index = multiChoiceSentence.Choices.IndexOf(choice);

            if (choice.Dialogue)
                if (!EditorUtility.DisplayDialog("Warning", "Do you want to overwrite the current dialogue slot?", "Yes", "No"))
                    return;

            // create new asset
            var dialogueAsset = property.serializedObject.targetObject;
            var path = AssetDatabase.GetAssetPath(dialogueAsset);
            var originalName = Path.GetFileNameWithoutExtension(path);
            var newDialogue = ScriptableObject.CreateInstance<Dialogue>();
            var newPath = $"{Path.GetDirectoryName(path)}/{originalName} {index}.asset";
            AssetDatabase.CreateAsset(newDialogue, newPath);

            // assign reference
            choice.Dialogue = newDialogue;
            //EditorUtility.SetDirty(dialogueAsset);
        }

        private static void AddDialogueInside(SerializedProperty property)
        {
            // init 
            var multiChoiceSentence = (MultipleChoicesSentence)property.GetParent();
            var choice = (MultipleChoicesSentence.Choice)property.GetObject();
            var index = multiChoiceSentence.Choices.IndexOf(choice);

            // create new asset
            var dialogueAsset = property.serializedObject.targetObject;
            var newDialogue = ScriptableObject.CreateInstance<Dialogue>();
            newDialogue.name = $"OnOption {index}";
            AssetDatabase.AddObjectToAsset(newDialogue, dialogueAsset);

            // assign reference
            choice.Dialogue = newDialogue;
        }

        //private static void DeleteDialogueInside(SerializedProperty property)
        //{
        //}

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property) + buttonGroupHeight;
        }
    }
}
