using System;
using System.Collections.Generic;

using NaughtyAttributes;

using UnityEngine;

namespace WeaponSystem
{
    static class Global
    {
        public const string AssetMenu = "Weapon System/";
        public const string AttackMenu = AssetMenu + "Attack/";
    }

    public class Weapon : ScriptableObject
    {
        [SerializeField] float cooldown;
        [SerializeField] protected float interval;
        [SerializeField] Attack attack;
        [SerializeReference] List<AttackAction> attacks;

        [Button]
        public void AddAttack() => EditorHelper.ShowTypesMenuToAdd(attacks, true);

        [SerializeField] GameObject prefab;
        [SerializeField] float angleOffset;

        protected float timer;

        public GameObject Prefab => prefab;
        public float AngleOffset => angleOffset;
        public IReadOnlyList<AttackAction> Attacks => attacks;

        // events
        public event Action onPressed, onHold, onReleased;

        public virtual void Init()
        {
            timer = 0;
        }

        public virtual void Update()
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        }

        public virtual void Attack(AttackContext context)
        {
            //attack.Execute(context);
            foreach (var attack in attacks) attack.Execute(context);
        }

        public virtual void OnPressed(AttackContext context)
        {
            onPressed?.Invoke();
        }

        public virtual void OnHold()
        {
            onHold?.Invoke();
        }

        public virtual void OnReleased(AttackContext context)
        {
            onReleased?.Invoke();
        }

        internal virtual void ResetWeapon() { }
    }
}
