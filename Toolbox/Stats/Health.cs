using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float max;

    // props
    public float Value
    {
        get => value;
        set
        {
            value = Mathf.Clamp(value, 0, max);
            if (value == this.value) return;

            var delta = value - this.value;
            this.value = value;

            OnChanged?.Invoke(delta);
            if (value == 0) OnDepleted?.Invoke();
        }
    }
    public float Max => max;

    // fields
    [SerializeField, ReadOnly] private float value;

    // events
    public event Action OnDepleted;
    public event Action<float> OnChanged;

    private void Awake() => value = max;
}
