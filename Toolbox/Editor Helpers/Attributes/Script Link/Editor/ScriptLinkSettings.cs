using System.IO;
using System.Linq;

using NaughtyAttributes;

using UnityEditor;

using UnityEngine;

namespace SkillSystem
{
    public class ScriptLinkSettings : ScriptableObject
    {
        [SerializeField] bool enableLink;
        public bool EnableLink => enableLink;

        [SerializeField] bool pingSource;
        public bool PingScript => pingSource;

        [SerializeField] bool autoCaching;
        public bool AutoCaching => autoCaching;

        [SerializeField] bool logCache;
        public bool LogCache => logCache;

        static ScriptLinkSettings instance;
        public static ScriptLinkSettings Instance
        {
            get
            {
                if (instance == null) instance = FindExistingObject();
                if (instance == null) instance = CreateNewObject();
                return instance;
            }
        }

        private static ScriptLinkSettings FindExistingObject()
        {
            var guids = AssetDatabase.FindAssets($"t:{nameof(ScriptLinkSettings)}");
            if (guids.Length == 0) return null;
            var path = AssetDatabase.GUIDToAssetPath(guids.First());
            return AssetDatabase.LoadAssetAtPath<ScriptLinkSettings>(path);
        }

        private static ScriptLinkSettings CreateNewObject()
        {
            var instance = CreateInstance<ScriptLinkSettings>();
            var script = MonoScript.FromScriptableObject(instance);
            var path = AssetDatabase.GetAssetPath(script);
            var folder = Path.GetDirectoryName(path);
            AssetDatabase.CreateAsset(instance, Path.Combine(folder, nameof(ScriptLinkSettings)) + ".asset");
            AssetDatabase.SaveAssets();
            Debug.Log($"Created {nameof(ScriptLinkSettings)} at {folder}");
            return instance;
        }

        private void OnEnable() => instance = this;

        const string MenuPath = "Tools/" + nameof(ScriptLinkSettings) + "/";

        [MenuItem(MenuPath + nameof(Ping))]
        public static void Ping() => EditorGUIUtility.PingObject(instance);

        [MenuItem(MenuPath + nameof(Select))]
        public static void Select() => AssetDatabase.OpenAsset(instance);

        [MenuItem(MenuPath + nameof(ManualCache))]
        public static void ManualCache() => ScriptLinkCache.CacheAllScripts();

        [Button]
        public void SetSingletonInstance() => instance = this;
    }
}