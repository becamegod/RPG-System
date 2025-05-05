using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public abstract class GraphicTransition : UITransition
    {
        [SerializeField] protected Graphic graphic;

        private new void Reset()
        {
            base.Reset();
            graphic = GetComponent<Graphic>();
        }

        //private void OnDestroy() => graphic.DOKill();
    }
}
