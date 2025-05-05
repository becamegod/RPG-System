using DG.Tweening;
using UnityEngine;

namespace UISystem
{
    public class ColorTransition : GraphicTransition
    {
        [SerializeField] Color activeColor = Color.white;
        [SerializeField] Color disabledColor = Color.gray;

        private Color inactiveColor;
        UIInteraction UI => interaction as UIInteraction;

        private new void Awake()
        {
            base.Awake();
            inactiveColor = graphic.color;
            UI.OnStatusChanged += OnStatusChanged;
        }

        private void OnDestroy() => UI.OnStatusChanged -= OnStatusChanged;

        protected override void OnExit()
        {
            base.OnExit();
            graphic.DOKill();
            graphic.DOColor(inactiveColor, exitDuration).SetEase(exitEase);
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            graphic.DOKill();
            graphic.DOColor(activeColor, enterDuration).SetEase(enterEase);
        }

        protected void OnStatusChanged()
        {
            var color = UI.Interactable ? UI.Highlighted ? activeColor : inactiveColor : disabledColor;
            graphic.DOKill();
            graphic.DOColor(color, enterDuration);
        }
    }
}
