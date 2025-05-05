using UnityEngine;

public class RelativeCanvasOrder : MonoBehaviour
{
    [SerializeField] int delta = 1;

    private void Awake()
    {
        var parentCanvas = GetComponentInParent<Canvas>();
        var canvas = gameObject.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = delta;
    }
}
