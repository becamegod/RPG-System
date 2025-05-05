using UnityEngine;

public class ProgressionUI : MonoBehaviour
{
    [SerializeField] Transform nodeParent;
    [SerializeField] bool isCenterMode;

    private int max;
    public int Max
    {
        get => max;
        set => max = value;
    }

    private int number = -1;
    private IProgressionNode[] visibleNodes;

    public int Number
    {
        get => number;
        set
        {
            value = Mathf.Clamp(value, 0, Max - 1);
            if (value == number) return;

            // get visible index
            var prevNodeIndex = GetVisibleNodeIndex();
            number = value;
            var nodeIndex = GetVisibleNodeIndex();

            // highlight
            if (prevNodeIndex != nodeIndex)
            {
                if (prevNodeIndex >= 0) visibleNodes[prevNodeIndex].Dehighlight();
                visibleNodes[nodeIndex].Highlight();
            }

            // update texts & disabled
            var startingNumber = number - nodeIndex;
            for (int i = 0; i < visibleNodes.Length; i++)
            {
                if (visibleNodes[i] is SpriteBaseProgressionNode)
                {
                    var node = visibleNodes[i] as SpriteBaseProgressionNode;
                    node.text = (startingNumber + i).ToString();
                }
                if (i > nodeIndex) visibleNodes[i].Disable();
            }
        }
    }

    private int GetVisibleNodeIndex()
    {
        if (isCenterMode) return GetVisibleNodeIndexWithCenterMode();
        return GetVisibleNodeIndexWithLoopMode();
    }

    private int GetVisibleNodeIndexWithLoopMode()
    {
        return Number % visibleNodes.Length;
    }

    private int GetVisibleNodeIndexWithCenterMode()
    {
        // left edge
        var middle = (visibleNodes.Length - 1) / 2;
        if (Number < middle) return Number;

        // right edge
        var rightEdge = Max - (visibleNodes.Length - middle);
        if (Number >= rightEdge) return Number - (Max - visibleNodes.Length);

        // middle node
        return middle;
    }

    private void Awake()
    {
        visibleNodes = GetComponentsInChildren<IProgressionNode>();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) Number++;
    }
#endif
}
