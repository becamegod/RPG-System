using UnityEngine;
using UnityEngine.InputSystem;

using WeaponSystem;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] AnimationCallback animationCallback;
    [SerializeField] LineRenderer trail;
    [SerializeField] private Transform projectileSpawnPoint;

    public Weapon weapon;

    // props
    public Transform ProjectileSpawnPoint => projectileSpawnPoint;

    private void Start()
    {
        weapon.Init();
    }

    private void Update()
    {
        weapon.Update();
    }

    public void PressWeapon(InputActionReference input) => weapon.OnPressed(new(input, animationCallback.GetComponent<Animator>()));

    public void ResetWeapon() => weapon.ResetWeapon();

    public void OnHitStart()
    {
        trail.enabled = true;
        hitbox.gameObject.SetActive(true);
    }

    public void OnHitEnd()
    {
        trail.enabled = false;
        hitbox.gameObject.SetActive(false);
    }
}
