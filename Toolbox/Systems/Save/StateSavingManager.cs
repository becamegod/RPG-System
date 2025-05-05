using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    public class StateSavingManager : Singleton<StateSavingManager>, ISavable
    {
        [SerializeField] private Dictionary<string, string> stateMap;

        private new void Awake()
        {
            base.Awake();
            stateMap = new();
        }

        public object Save() => new StateSaveData(stateMap);

        public void Load(string dataString)
        {
            var saveData = JsonUtility.FromJson<StateSaveData>(dataString);
            stateMap = new();
            foreach (var entry in saveData.States) stateMap[entry.Id] = entry.Data;
        }

        public T GetState<T>(string id) where T : SavingState
        {
            var state = stateMap.ContainsKey(id) ? JsonUtility.FromJson<T>(stateMap[id]) : null;
            if (state != null) state.OnChanged += newState => stateMap[id] = JsonUtility.ToJson(newState);
            return state;
        }

        public void RegisterState(string id, SavingState state)
        {
            if (stateMap.ContainsKey(id)) Debug.Log($"Possible save bug: id {id} is already existed and is going to be overwritten");
            stateMap[id] = JsonUtility.ToJson(state);
            state.OnChanged += newState => stateMap[id] = JsonUtility.ToJson(newState);
        }
    }

    [Serializable]
    public abstract class SavingState
    {
        public event Action<SavingState> OnChanged;

        public SavingState(Action<SavingState> onChanged) => OnChanged = onChanged;

        public void RaiseEvent(SavingState state) => OnChanged?.Invoke(state);

        public abstract void Load(string dataString);
    }

    [Serializable]
    public class StateSaveData
    {
        [Serializable]
        public struct Entry
        {
            [SerializeField] string id;
            [SerializeField] string data;

            public Entry(string id, string data)
            {
                this.id = id;
                this.data = data;
            }

            public string Id => id;
            public string Data => data;
        }

        [SerializeField] List<Entry> states;

        public List<Entry> States => states;

        public StateSaveData(Dictionary<string, string> stateMap)
        {
            states = new();
            foreach (var (key,value) in stateMap) States.Add(new Entry(key, value));
        }
    }
}
