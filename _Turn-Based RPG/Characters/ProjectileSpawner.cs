using UnityEngine;
using UnityEngine.Events;

using WeaponSystem;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] Projectile prefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] BattleCharacter character;
    [SerializeField] UnityEvent onProjectileHit;

    private void Reset() => character = GetComponentInParent<BattleCharacter>();

    public void Shoot()
    {
        foreach (var t in character.Targets)
        {
            var target = t.GetComponentInChildren<BattleCharacter>();
            var projectile = Instantiate(prefab);
            projectile.Launch(spawnPoint.position, target.Center.position);
            projectile.Hitbox.OnHit += OnProjectileHit;

            void OnProjectileHit(GameObject gameObject, Vector3 position)
            {
                projectile.Hitbox.OnHit -= OnProjectileHit;
                onProjectileHit.Invoke();
            }
        }
    }
}
