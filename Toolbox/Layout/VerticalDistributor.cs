using UnityEngine;

[ExecuteAlways]
public class VerticalDistributor : MonoBehaviour
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
        var startZ = (n % 2 == 1) ? (n / 2) * distance : (n / 2 - .5f) * distance;
        var pos = transform.TransformPoint(Vector3.zero);
        pos.z += startZ;
        for (int i = 0; i < n; i++)
        {
            var child = transform.GetChild(i);
            child.position = pos;
            pos.z -= distance;
        }
    }
}
