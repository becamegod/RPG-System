using System.Collections.Generic;

using UnityEngine;

public class SimplePool<T> where T : Component
{
    public readonly Transform parent;
    private readonly T prototype;
    private readonly Queue<T> freeObjects;
    private readonly List<T> usedObjects;

    public SimplePool(T prototype)
    {
        this.prototype = prototype;
        freeObjects = new Queue<T>();
        usedObjects = new List<T>();
    }

    public SimplePool(T prototype, string name) : this(prototype) => parent = new GameObject(name).transform;

    public SimplePool(T prototype, Transform poolParent) : this(prototype) => parent = poolParent;

    public T Spawn()
    {
        if (freeObjects.Count == 0) freeObjects.Enqueue(Object.Instantiate(prototype, parent));
        var obj = freeObjects.Dequeue();
        obj.gameObject.SetActive(true);
        usedObjects.Add(obj);
        return obj;
    }

    public T Spawn(Vector3 position, Quaternion rotation)
    {
        var obj = Spawn();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public T Spawn(Vector3 position) => Spawn(position, Quaternion.identity);

    public void Despawn(T obj)
    {
        obj.gameObject.SetActive(false);
        freeObjects.Enqueue(obj);
        usedObjects.Remove(obj);
    }

    public void DespawnAll()
    {
        while (usedObjects.Count > 0) Despawn(usedObjects[0]);
    }

    public void Destroy() => Object.Destroy(parent.gameObject);
}

public class BasePools<C, T> : AutoSingleton<C> where T : Component where C : MonoBehaviour
{
    Dictionary<T, SimplePool<T>> poolMap;

    private void Awake() => poolMap = new();

    public T Spawn(T type, Vector3 position)
    {
        var pool = GetPool(type);
        return pool.Spawn(position);
    }

    public void Despawn(T type, T obj)
    {
        var pool = GetPool(type);
        pool.Despawn(obj);
    }

    SimplePool<T> GetPool(T type)
    {
        if (!poolMap.ContainsKey(type)) poolMap[type] = new(type, transform);
        return poolMap[type];
    }
}