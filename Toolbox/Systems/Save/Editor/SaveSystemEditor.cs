using System.IO;
using UnityEditor;
using UnityEngine;

namespace SaveSystem.Editor
{
    public class SaveSystemEditor
    {
        [MenuItem("Game/Open Save Folder", false, 30)]
        private static void OpenSaveFolder()
        {
            EditorUtility.RevealInFinder(SaveManager.SaveFolderPath);
        }

        [MenuItem("Game/Clear All Saves", false, 30)]
        private static void ClearAllSaves()
        {
            var folder = new DirectoryInfo(SaveManager.SaveFolderPath);
            foreach (var file in folder.GetFiles()) file.Delete();
            Debug.Log("Save data files cleared");
        }
    }
}
