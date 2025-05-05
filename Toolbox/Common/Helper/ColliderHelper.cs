using UnityEngine;

public static class ColliderHelper
{
    public static Vector3 GetRandomPointInsideCollider(this BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boxCollider.center;
        return boxCollider.transform.TransformPoint(point);
    }

    public static Vector3 GetRandomPointInsideCollider(this BoxCollider boxCollider, float padding)
    {
        var extents = boxCollider.transform.TransformVector(boxCollider.size / 2f);
        var center = boxCollider.transform.TransformPoint(boxCollider.center);
        extents.x -= Mathf.Sign(extents.x) * padding;
        extents.y -= Mathf.Sign(extents.y) * padding;
        extents.z -= Mathf.Sign(extents.z) * padding;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + center;
        return point;
    }
}
