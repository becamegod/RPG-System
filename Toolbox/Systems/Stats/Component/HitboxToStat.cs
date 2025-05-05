using StatsSystem;

using UnityEngine;

public class HitboxToStat : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] ScriptableReference stat;

    private void Reset() => hitbox ??= GetComponent<Hitbox>();

    private void OnEnable() => hitbox.OnHit += DealDamage;

    private void OnDisable() => hitbox.OnHit -= DealDamage;

    public void DealDamage(GameObject target, Vector3 _)
    {
        var stats = target.GetComponentInChildren<StatsOwner>().Stats;
        stats[stat.name].Value -= hitbox.Damage;
    }
}
