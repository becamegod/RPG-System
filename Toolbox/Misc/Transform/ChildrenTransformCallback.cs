using System;
using UnityEngine;

public class ChildrenTransformCallback : MonoBehaviour
{
    public event Action OnChanged;
    private void OnTransformChildrenChanged() => OnChanged?.Invoke();
}