using UnityEngine;
using UnityEngine.Events;

using WeaponSystem;

public class ChargeEvent : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] UnityEvent onStartCharging;
    [SerializeField] UnityEvent onStopCharging;

    private void OnEnable()
    {
        weapon.onPressed += OnPressed;
        weapon.onReleased += OnReleased;
    }

    private void OnDisable()
    {
        weapon.onPressed -= OnPressed;
        weapon.onReleased -= OnReleased;
    }

    private void OnDestroy()
    {
        enabled = false;
    }

    private void OnPressed() => onStartCharging.Invoke();
    private void OnReleased() => onStopCharging.Invoke();
}
