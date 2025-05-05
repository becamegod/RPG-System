using System.Collections.Generic;
using UnityEngine;

public class BehaviorIdManager<T> : Singleton<BehaviorIdManager<T>> where T : Object
{
    private Dictionary<string, T> map;

    private new void Awake()
    {
        base.Awake();
        map = new();
    }

    internal void RegisterId(string id, T target)
    {
        if (map.ContainsKey(id))
        {
            Debug.LogWarning($"Register error: Behavior with id {id} is already existed");
            return;
        }
        map[id] = target;
    }

    internal void DeregisterId(string id)
    {
        if (map.ContainsKey(id)) map.Remove(id);
        else Debug.LogWarning($"Deregister error: Behavior with id {id} is not existed");
    }

    public T GetById(string id)
    {
        if (map.ContainsKey(id)) return map[id];
        Debug.LogWarning($"Query error: Behavior with id {id} is not existed");
        return null;
    }
}

