using System;

using UnityEngine;

[CreateAssetMenu(fileName = "New Stat", menuName = "Stat")]
[Serializable]
public class Stat : ScriptableObject
{
    [SerializeField] float value;
    [SerializeField] float min = 0;
    [SerializeField] float max = float.MaxValue;

    public float Value
    {
        get => value;
        set
        {
            value = Mathf.Clamp(value, min, max);
            if (this.value == value) return;
            this.value = value;
            OnChanged?.Invoke();
        }
    }

    public float Max
    {
        get => max;
        set
        {
            if (max == value) return;
            max = value;
            OnChanged?.Invoke();
        }
    }

    public event Action OnChanged;

    public void CopyFrom(Stat other)
    {
        value = other.value;
        min = other.min;
        max = other.max;
    }
}
