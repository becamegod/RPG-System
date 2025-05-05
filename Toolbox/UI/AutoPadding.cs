using UnityEngine;
using UnityEngine.UI;

public class AutoPadding : MonoBehaviour
{
    [ContextMenu("Pad")]
    public void Pad()
    {
        var layout = GetComponent<GridLayoutGroup>();
        var parentWidth = transform.parent.GetComponent<RectTransform>().rect.width;
        var nativeWidth = layout.constraintCount * layout.cellSize.x + (layout.constraintCount - 1) * layout.spacing.x;
        var widthDelta = parentWidth - nativeWidth;
        layout.padding.left = (int)widthDelta / 2;
        layout.padding.right = (int)widthDelta / 2;
        layout.enabled = false;
        layout.enabled = true;
    }
}