using DG.Tweening;
using UnityEngine;

namespace UISystem
{
    [RequireComponent(typeof(UIAnimation))]
    public class MenuCursor : Singleton<MenuCursor>
    {
        [SerializeField] Vector3 offset;
        [SerializeField] float transitionDuration = .2f;
        [SerializeField] Ease ease = Ease.OutQuint;
        [SerializeField] bool continuousMode;
        [SerializeField, ShowWhen("continuousMode")] float speed = 5;

        private Vector3 position;
        public Vector3 Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                if (!continuousMode) transform.DOMove(position, transitionDuration).SetEase(ease);
            }
        }


        private bool visible;
        private UIInteraction target;
        private UIAnimation visibility;

        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                if (visible) visibility.Show();
                else visibility.Hide();
            }
        }

        //private Transform follow;
        //public void MoveTo(Transform target, Vector3 offset)
        //{
        //    follow = target;
        //    this.offset = offset;
        //}

        private new void Awake()
        {
            base.Awake();
            visibility = GetComponent<UIAnimation>();
        }

        public void MoveTo(UIInteraction target) => this.target = target;

        public void MoveToInstantly(UIInteraction target)
        {
            this.target = target;
            transform.position = target.GetLeftSide() + offset;
        }

        private void Update()
        {
            if (!continuousMode || !target) return;
            //var pos = follow.position + offset;
            var pos = target.GetLeftSide() + offset;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
        }
    }
}
