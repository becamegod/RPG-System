using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    public class UIIDManager : Singleton<UIIDManager>
    {
        private Dictionary<string, BaseUI> uiMap;

        private new void Awake()
        {
            base.Awake();
            uiMap = new();
        }

        internal void RegisterId(string id, BaseUI target)
        {
            if (uiMap.ContainsKey(id))
            {
                Debug.LogWarning($"Register error: UI with id {id} is already existed");
                return;
            }
            uiMap[id] = target;
        }

        internal void DeregisterId(string id)
        {
            if (uiMap.ContainsKey(id)) uiMap.Remove(id);
            else Debug.LogWarning($"Deregister error: UI with id {id} is not existed");
        }

        public BaseUI GetUIWithId(string id)
        {
            if (uiMap.ContainsKey(id)) return uiMap[id];
            Debug.LogWarning($"Query error: UI with id {id} is not existed");
            return null;
        }
    }
}