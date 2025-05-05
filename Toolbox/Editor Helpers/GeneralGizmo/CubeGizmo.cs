using UnityEngine;

public class CubeGizmo : GeneralGizmo
{
    public Vector3 size;

    private void OnDrawGizmos()
    {
        Gizmos.color = manager.color;
        Gizmos.DrawCube(transform.position, size);
    }
}
