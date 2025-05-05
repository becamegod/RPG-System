using UnityEngine;

public class PositionGizmo : GeneralGizmo
{
    private void OnDrawGizmos()
    {
        Gizmos.color = manager.color;
        Gizmos.DrawSphere(transform.position, manager.sphereSize);
    }
}
