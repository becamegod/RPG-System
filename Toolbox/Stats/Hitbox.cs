using System;
using System.Collections.Generic;

using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] FilteredCollisionCallback collisionCallback;
    [SerializeField] float damage = 1;

    [Header("Debug")]
    [SerializeField] bool logPotentialHit;
    [SerializeField] bool logActualHit;

    // fields
    private List<GameObject> hits;

    // events
    public event Action<GameObject, Vector3> OnHit;

    // props
    public float Damage
    {
        get => damage;
        set => damage = Mathf.Max(value, 0);
    }

    private void Reset() => collisionCallback ??= GetComponent<FilteredCollisionCallback>();

    private void Awake() => hits = new();

    private void OnEnable()
    {
        hits.Clear();
        collisionCallback.OnEntered += ProcessCollision;
    }

    private void OnDisable() => collisionCallback.OnEntered -= ProcessCollision;

    private void ProcessCollision(BaseCollider collider)
    {
        var subject = collider.Transform.gameObject;
        if (logPotentialHit) Debug.Log($"Potential Hit: {subject.name}");

        // filter
        if (hits.Contains(subject)) return;
        hits.Add(subject);
        if (logActualHit) Debug.Log($"Actual Hit: {subject.name}");

        // event
        OnHit?.Invoke(subject, collider.bounds.ClosestPoint(collisionCallback.transform.position));
    }
}
