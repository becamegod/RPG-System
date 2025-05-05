using UnityEngine;
using UnityEngine.Events;

public class HitboxEvent : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] UnityEvent<GameObject, Vector3> callback;

    private void Reset() => hitbox ??= GetComponent<Hitbox>();

    private void OnEnable() => hitbox.OnHit += OnHit;

    private void OnDisable() => hitbox.OnHit -= OnHit;

    private void OnHit(GameObject target, Vector3 contactPoint) => callback.Invoke(target, contactPoint);
}
