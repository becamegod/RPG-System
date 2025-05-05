using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class BaseCollider
{
    [SerializeField] Transform transform;
    public Transform Transform => transform;

    public readonly Bounds bounds;

    public readonly List<Vector3> contactPoints;
    public readonly List<Vector2> contactPoints2D;

    public BaseCollider(Collider collider)
    {
        bounds = collider.bounds;
        transform = collider.transform;
    }

    public BaseCollider(Collider2D collider)
    {
        bounds = collider.bounds;
        transform = collider.transform;
    }

    public BaseCollider(Collision collision) : this(collision.collider)
    {
        var contacts = new List<ContactPoint>();
        collision.GetContacts(contacts);
        contactPoints = contacts.ConvertAll(contact => contact.point);
    }

    public BaseCollider(Collision2D collision) : this(collision.collider)
    {
        var contacts = new List<ContactPoint2D>();
        collision.GetContacts(contacts);
        contactPoints2D = contacts.ConvertAll(contact => contact.point);
    }
}

#region Wrappers
//public interface ICollider
//{
//    bool Enabled { get; set; }
//    Vector3 Position { get; }
//}

//public class ColliderWrapper : ICollider
//{
//    private Collider collider;

//    public ColliderWrapper(Collider collider) => this.collider = collider;

//    public bool Enabled
//    {
//        get => collider.enabled;
//        set => collider.enabled = value;
//    }

//    public Vector3 Position => collider.transform.position;
//}

//public class Collider2DWrapper : ICollider
//{
//    private Collider2D collider2D;

//    public Collider2DWrapper(Collider2D collider2D) => this.collider2D = collider2D;

//    public bool Enabled
//    {
//        get => collider2D.enabled;
//        set => collider2D.enabled = value;
//    }

//    public Vector3 Position => collider2D.transform.position;
//}
#endregion