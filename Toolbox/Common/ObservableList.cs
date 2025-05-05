using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class ObservableList<T> : IList<T>
{
    [SerializeReference, SerializeField] List<T> list = new();
    public event Action<T> OnAdded;
    public event Action<T> OnRemoved;
    public ObservableList() => list = new();
    public void Add(T item)
    {
        list.Add(item);
        OnAdded?.Invoke(item);
    }
    public bool Remove(T item)
    {
        if (list.Remove(item))
        {
            OnRemoved?.Invoke(item);
            return true;
        }
        return false;
    }

    #region IList interface boring implementation
    public int Count => list.Count;
    public bool IsReadOnly => true;

    public T this[int index]
    {
        get => list[index];
        set => list[index] = value;
    }

    public int IndexOf(T item) => list.IndexOf(item);

    public void Insert(int index, T item)
    {
        list.Insert(index, item);
        OnAdded?.Invoke(item);
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < list.Count)
        {
            T item = list[index];
            list.RemoveAt(index);
            OnRemoved?.Invoke(item);
        }
    }

    public void Clear()
    {
        foreach (var item in list) OnRemoved?.Invoke(item);
        list.Clear();
    }

    public bool Contains(T item) => list.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
#endregion
}