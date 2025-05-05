using DG.Tweening;

using UnityEngine;

namespace UISystem
{
    public abstract class UITransition : MonoBehaviour
    {
        [SerializeField] protected BaseEnterExitEffect interaction;
        [SerializeField] protected float enterDuration = .5f;
        [SerializeField] protected float exitDuration = .5f;
        [SerializeField] protected Ease enterEase = Ease.OutQuint;
        [SerializeField] protected Ease exitEase = Ease.OutQuint;
        [SerializeField] bool listenEnterEvent = true;
        [SerializeField] bool listenToggleEvent;

        public float EnterDuration => enterDuration;
        public float ExitDuration => exitDuration;

        protected void Reset() => interaction = GetComponentInParent<BaseEnterExitEffect>();

        private void OnValidate()
        {
            if (listenToggleEvent && interaction is not IToggleable)
            {
                Debug.LogWarning($"{interaction} is not {nameof(IToggleable)}");
                listenToggleEvent = false;
            }
        }

        protected void Awake()
        {
            if (listenEnterEvent)
            {
                interaction.OnEnter += OnEnter;
                interaction.OnExit += OnExit;
            }
            if (listenToggleEvent)
            {
                var toggleable = interaction as IToggleable;
                toggleable.OnToggledOn += OnEnter;
                toggleable.OnToggledOff += OnExit;
            }
        }

        protected virtual void OnExit() {}
        protected virtual void OnEnter() {}
    }
}
