using UnityEngine;

public class BoundCenter : MonoBehaviour
{
    [SerializeField] Renderer targetRenderer;
    [SerializeField] Vector3 offset;

    private void Update()
    {
        transform.position = targetRenderer.bounds.center + offset;
    }
}
