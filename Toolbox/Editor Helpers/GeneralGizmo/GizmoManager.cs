using UnityEngine;

public class GizmoManager : MonoBehaviour
{
    public float sphereSize = .1f;
    public Color color = new Color(0, 0, 1, .5f);

    private static GizmoManager instance;

    public static GizmoManager Instance
    {
        get
        {
            if (!instance) instance = new GameObject("GizmoManager", typeof(GizmoManager)).GetComponent<GizmoManager>();
            return instance;
        }
    }
}
