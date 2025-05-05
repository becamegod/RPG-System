using UnityEngine;

public class GeneralGizmo : MonoBehaviour
{
    protected GizmoManager manager;

    private void Awake()
    {
        manager = GizmoManager.Instance;
    }
}
