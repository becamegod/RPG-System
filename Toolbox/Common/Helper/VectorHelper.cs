using UnityEngine;

public static class VectorHelper
{
    // vector 3
    public static float MaxComponent(this Vector3 a) => Mathf.Max(Mathf.Max(a.x, a.y), a.z);
    public static float MaxComponentXZ(this Vector3 a) => Mathf.Max(a.x, a.z);
    public static float MinComponent(this Vector3 a) => Mathf.Min(Mathf.Min(a.x, a.y), a.z);
    public static float MinComponentXZ(this Vector3 a) => Mathf.Min(a.x, a.z);

    public static Vector3 UniformVector3(float value) => new(value, value, value);
    public static Vector3 WithX(this Vector3 vector, float value) => new(value, vector.y, vector.z);
    public static Vector3 WithY(this Vector3 vector, float value) => new(vector.x, value, vector.z);
    public static Vector3 WithZ(this Vector3 vector, float value) => new(vector.x, vector.y, value);
    public static Vector3 AddX(this Vector3 vector, float value) => new(vector.x + value, vector.y, vector.z);
    public static Vector3 AddY(this Vector3 vector, float value) => new(vector.x, vector.y + value, vector.z);
    public static Vector3 AddZ(this Vector3 vector, float value) => new(vector.x, vector.y, vector.z + value);


    // vector 2
    public static float MinComponent(this Vector2 a) => Mathf.Min(a.x, a.y);
    public static float MaxComponent(this Vector2 a) => Mathf.Max(a.x, a.y);

    public static Vector2 WithX(this Vector2 vector, float value) => new(value, vector.y);
    public static Vector2 WithY(this Vector2 vector, float value) => new(vector.x, value);
    public static Vector2 AddX(this Vector2 vector, float value) => new(vector.x + value, vector.y);
    public static Vector2 AddY(this Vector2 vector, float value) => new(vector.x, vector.y + value);
}