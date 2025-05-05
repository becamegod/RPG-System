using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton : MonoBehaviour
{
    [SerializeField] string id;

    private static List<string> existedIds;

    private void Awake()
    {
        if (existedIds == null) existedIds = new();
        if (existedIds.Contains(id))
        {
            id = "";
            DestroyImmediate(gameObject);
            return;
        }
        existedIds.Add(id);
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        if (id == "") return;
        existedIds.Remove(id);
    }

    private void Reset()
    {
        id = gameObject.name;
    }
}
