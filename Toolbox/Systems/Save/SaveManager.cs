using InventorySystem;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UISystem;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        [Header("Tweaking")]
        [SerializeField] int slotNumber = 10;

        // events
        internal event Action<int> OnSlotChanged;

        // props
        private Camera Cam => cam != null ? cam : (cam = Camera.main);
        internal List<object> SaveDatas => saveDatas;
        internal int SlotNumber => slotNumber;

        // const
        public static string SaveFolderPath => Path.Combine(Application.persistentDataPath, "Saves");

        // fields
        private Camera cam;
        private List<object> saveDatas;

        private new void Awake()
        {
            base.Awake();
            LoadAllSaveDatas();
        }

        public SaveResult Save(object data, int slotIndex, bool forced = false)
        {
            // create directory & check file existed
            if (!Directory.Exists(SaveFolderPath)) Directory.CreateDirectory(SaveFolderPath);
            var filePath = GetFilePath(slotIndex);
            if (!forced && File.Exists(filePath)) return SaveResult.FileExisted;

            try
            {
                saveDatas[slotIndex] = data;
                File.WriteAllText(filePath, JsonUtility.ToJson(data));
                OnSlotChanged?.Invoke(slotIndex);
                return SaveResult.Succeed;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return SaveResult.Failed;
            }
        }

        public object Load(int slotIndex) => ReadSaveFile(slotIndex);

        public T Load<T>(int slotIndex)
        {
            var filePath = GetFilePath(slotIndex);
            if (File.Exists(filePath))
            {
                var saveData = JsonUtility.FromJson<T>(File.ReadAllText(filePath));
                ProcessFields(saveData, new(), new(), OnAfterLoading);
                return saveData;
            }
            return default(T);

            static void OnAfterLoading(ISaveLoadProcessing subject) => subject.OnAfterLoading();
        }

        private void LoadAllSaveDatas()
        {
            saveDatas = new();
            for (int slotIndex = 0; slotIndex < slotNumber; slotIndex++)
                saveDatas.Add(ReadSaveFile(slotIndex));
        }

        internal object ReadSaveFile(int slotIndex)
        {
            var filePath = GetFilePath(slotIndex);
            if (File.Exists(filePath))
            {
                var saveData = JsonUtility.FromJson<object>(File.ReadAllText(filePath));
                ProcessFields(saveData, new(), new(), OnAfterLoading);
                return saveData;
            }
            return null;

            static void OnAfterLoading(ISaveLoadProcessing subject) => subject.OnAfterLoading();
        }

        internal DeleteResult Delete(int slotIndex)
        {
            if (saveDatas[slotIndex] == null) return DeleteResult.Failed;
            File.Delete(GetFilePath(slotIndex));
            saveDatas[slotIndex] = null;
            OnSlotChanged?.Invoke(slotIndex);
            return DeleteResult.Succeed;
        }

        private string GetFilePath(int slotIndex) => Path.Combine(SaveFolderPath, $"SaveData_{slotIndex}.json");

        internal static string GetSceneName(int sceneIndex)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            if (scenePath == "") return "Scene not in build";
            return scenePath[(scenePath.LastIndexOf('/') + 1)..scenePath.LastIndexOf(".unity")];
        }

        public static void ProcessFields(object obj)
        {
            ProcessFields(obj, new(), new(), OnBeforeSaving);
            void OnBeforeSaving(ISaveLoadProcessing subject) => subject.OnBeforeSaving();
        }

        public static void ProcessFields(object obj, List<Type> existedTypes, List<object> processedObjs, Action<ISaveLoadProcessing> action)
        {
            if (obj == null || processedObjs.Contains(obj)) return;
            var type = obj.GetType();
            if (existedTypes.Contains(type) && obj is not ISaveLoadProcessing) return;
            existedTypes.Add(type);
            if (obj is ISaveLoadProcessing subject) action(subject);
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            foreach (var field in fields)
            {
                var val = field.GetValue(obj);
                if (val is IList list) foreach (var item in list) ProcessFields(item, existedTypes, processedObjs, action);
                else ProcessFields(val, existedTypes, processedObjs, action);
            }
        }
    }

    public enum SaveResult
    {
        Succeed,
        FileExisted,
        Failed
    }

    enum LoadResult
    {
        Succeed,
        Failed
    }

    enum DeleteResult
    {
        Succeed,
        Failed
    }
}
