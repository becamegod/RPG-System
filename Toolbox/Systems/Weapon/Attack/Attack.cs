using System;
using System.Collections.Generic;

using NaughtyAttributes;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace WeaponSystem
{
    public class Attack : ScriptableObject
    {
        public virtual void Execute(AttackContext context)
        {

        }
    }

    [Serializable]
    public abstract class SubAction : AutoLabeled, INewButton
    {
        public abstract void Execute(AttackContext context);
    }

    [Serializable]
    public class SetActiveAction : SubAction
    {
        [SerializeField] GameObject gameObject;
        [SerializeField] bool active;
        public override void Execute(AttackContext context) => gameObject.SetActive(active);
    }

    [Serializable]
    public class SetTriggerAction : SubAction
    {
        [SerializeField] Animator animator;
        [SerializeField] string trigger;
        public override void Execute(AttackContext context) => animator.SetTrigger(trigger);
    }

    [Serializable]
    public class PlayVisualEffectAction : SubAction
    {
        [SerializeField] VisualEffect visualEffect;
        public override void Execute(AttackContext context) => visualEffect.Play();
    }

    [Serializable]
    public class AttackAction
    {
        [SerializeField, ReadOnly] string label;
        [SerializeField] float damage;
        [SerializeReference, NewButton] List<SubAction> actions;

        [Button]
        public void AddAction() => EditorHelper.ShowTypesMenuToAdd(actions, false);

        public float Damage => damage;

        public AttackAction() => label = GetType().Name.SplitPascalCase();

        public virtual void Execute(AttackContext context)
        {
            foreach (var action in actions) action.Execute(context);
        }
    }

    public class AttackContext
    {
        Transform source;
        public Transform Source => source;

        Collider2D destination;
        public Collider2D Destination => destination;

        private Vector2 direction;
        public Vector2 Direction => direction;

        private InputActionReference input;
        public InputActionReference Input => input;

        private Animator weaponAnimator;
        public Animator WeaponAnimator => weaponAnimator;

        public float rate = 1f;

        public AttackContext(InputActionReference input, Animator animator)
        {
            this.input = input;
            weaponAnimator = animator;
        }

        public AttackContext(Animator animator) => weaponAnimator = animator;

        public AttackContext(Transform source, Collider2D destination, Vector2 direction, Animator animator)
        {
            this.source = source;
            this.destination = destination;
            this.direction = direction;
            weaponAnimator = animator;
        }
    }
}
