using System;

using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = Global.AttackMenu + "Generic Attack")]
    public class GenericAttack : ScriptableObject
    {
        public event Action<Context> OnStarted, OnEnded;

        public void End() => OnEnded?.Invoke(new(this));

        public void Start() => OnStarted?.Invoke(new(this));

        public class Context
        {
            public readonly GenericAttack attack;

            public Context(GenericAttack attack) => this.attack = attack;
        }
    }
}
