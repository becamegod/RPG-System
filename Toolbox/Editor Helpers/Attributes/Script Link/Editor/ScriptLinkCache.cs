using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEditor;

using UnityEngine;

namespace SkillSystem
{
    [InitializeOnLoad]
    public static class ScriptLinkCache
    {
        private static readonly Dictionary<string, string> classToFileMap = new();
        private static bool cached;

        static ScriptLinkCache()
        {
            if (!ScriptLinkSettings.Instance.AutoCaching) return;
            CacheAllScripts();
        }

        public static void CacheAllScripts()
        {
            classToFileMap.Clear();
            var allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

            foreach (var scriptPath in allScripts)
            {
                var assetPath = "Assets" + scriptPath.Replace(Application.dataPath, "").Replace("\\", "/");
                var scriptText = File.ReadAllText(scriptPath);

                var matches = Regex.Matches(scriptText, @"\b(class|struct|interface)\s+(\w+)");

                foreach (Match match in matches)
                {
                    var className = match.Groups[2].Value;
                    classToFileMap[className] = assetPath;
                }
            }
            Debug.Log("Script link cached");
            cached = true;

            if (ScriptLinkSettings.Instance.LogCache) LogCacheToFile();
        }

        private static void LogCacheToFile()
        {
            // get folder
            var scriptPath = "";
            var script = MonoScript.FromScriptableObject(ScriptLinkSettings.Instance);
            if (script != null) scriptPath = AssetDatabase.GetAssetPath(script);
            else
            {
                var guid = AssetDatabase.FindAssets($"{nameof(ScriptLinkCache)} t:MonoScript").First();
                scriptPath = AssetDatabase.GUIDToAssetPath(guid);
            }
            var folder = Path.GetDirectoryName(scriptPath);
            var logPath = Path.Combine(folder, "ScriptCache.txt");

            // write file
            using (var writer = new StreamWriter(logPath, false)) // Overwrite existing file
            {
                foreach (var entry in classToFileMap) writer.WriteLine($"{entry.Key} -> {entry.Value}");
            }
            Debug.Log($"ScriptCache logged to {logPath}");
        }

        public static bool TryGetScriptPath(string className, out string path)
        {
            if (!cached) CacheAllScripts();
            return classToFileMap.TryGetValue(className, out path);
        }
    }
}