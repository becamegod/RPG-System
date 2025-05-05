using System;
using UnityEngine;

public class MouseclickCallback : MonoBehaviour
{
    public Action onMouseUpAsButton, onMouseDown, onMouseUp,
        onMouseEnter, onMouseExit, onMouseOver, onMouseDrag;

    private void OnMouseUpAsButton() => onMouseUpAsButton?.Invoke();
    private void OnMouseDown() => onMouseDown?.Invoke();
    private void OnMouseUp() => onMouseUp?.Invoke();
    private void OnMouseEnter() => onMouseEnter?.Invoke();
    private void OnMouseExit() => onMouseExit?.Invoke();
    private void OnMouseOver() => onMouseOver?.Invoke();
    private void OnMouseDrag() => onMouseDrag?.Invoke();
}
