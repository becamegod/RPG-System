using UnityEngine;

public class ComponentCacher<T> where T : Component
{
    private T cache;
    public T Data
    {
        get
        {
            if (!cache) cache = holder.GetComponentInChildren<T>();
            return cache;
        }
    }

    private readonly MonoBehaviour holder;
    public ComponentCacher(MonoBehaviour holder)
    {
        this.holder = holder;
    }
}