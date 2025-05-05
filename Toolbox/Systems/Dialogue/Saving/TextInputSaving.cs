#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "TextInputSaving", menuName = "Dialogue/TextInputSaving")]
    public class TextInputSaving : ScriptableObject
    {
        [SerializeField] string key;
        [SerializeField] string styleTag;
        [ReadOnly, SerializeField] internal string text;
        public string Text => text;
        public string StyleTag => styleTag;
        public string Key => key;

#if UNITY_EDITOR
        private void Reset()
        {
            var assetPath = AssetDatabase.GetAssetPath(this);
            var displayName = Path.GetFileNameWithoutExtension(assetPath);
            key = styleTag = displayName;
        }
#endif
    }
}
