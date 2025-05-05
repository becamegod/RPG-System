using UnityEngine;

public class SpawnVFXOnHit : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] VisualEffectEvent visualEffect;

    private void OnEnable() => hitbox.OnHit += SpawnVFX;
    private void OnDisable() => hitbox.OnHit -= SpawnVFX;

    private void SpawnVFX(GameObject @object, Vector3 position) => VisualEffectPools.Play(visualEffect, position);
}
