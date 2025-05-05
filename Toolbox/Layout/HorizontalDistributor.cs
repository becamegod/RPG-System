using UnityEngine;

[ExecuteAlways]
public class HorizontalDistributor : MonoBehaviour
{
    [SerializeField] float distance;
    [SerializeField] bool updateInEditMode;

    public int ChildCount => transform.childCount;
    public float Distance
    {
        get => distance;
        set
        {
            if (value == distance) return;
            distance = value;
            Arrange();
        }
    }

    private void Awake()
    {
        Arrange();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying && !updateInEditMode) return;
        Arrange();
    }

    private void OnTransformChildrenChanged()
    {
        OnValidate();
    }

    private void Arrange()
    {
        var n = transform.childCount;
        var startX = (n % 2 == 1) ? (n / 2) * distance : (n / 2 - .5f) * distance;
        Vector3 pos = new Vector3(-startX, 0, 0);
        for (int i = 0; i < n; i++)
        {
            var child = transform.GetChild(i);
            child.localPosition = pos;
            pos.x += distance;
        }
    }
}
