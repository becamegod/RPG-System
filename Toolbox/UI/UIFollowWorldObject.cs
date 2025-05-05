using UnityEngine;

public class UIFollowWorldObject : MonoBehaviour
{
    [SerializeField] float defaultElevation = .5f;

    // props
    private Camera Cam => cam != null ? cam : (cam = Camera.main);

    // fields
    private Camera cam;
    private Transform target;
    private float currentElevation;
    private Vector3 worldPos;
    private Vector3 screenPos;
    private Vector3 prevScreenPos;

    public void Follow(Transform target, float elevation = 0)
    {
        this.target = target;
        currentElevation = elevation != 0 ? elevation : defaultElevation;
        Update();
    }

    public void Unfollow()
    {
        Follow(null);
        prevScreenPos = default;
    }

    private void Update()
    {
        if (!target) return;
        worldPos = target.position.AddY(currentElevation * target.localScale.x);
        screenPos = Cam.WorldToScreenPoint(worldPos);
        if (screenPos == prevScreenPos) return;
        transform.position = screenPos;
        prevScreenPos = screenPos;
    }
}
