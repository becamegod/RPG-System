using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class Menu : BaseUI, INavigatable, ISelectable, IExitBlocking, IExitListener
    {
        enum Scheme
        {
            Vertical,
            Horizontal,
            Grid,
            Explicit,
        }

        [SerializeField] Scheme scheme;
        [SerializeField] bool logWarning = false;
        public bool showCursor = true;
        [SerializeField] bool canGoBack = true;
        [SerializeField] bool resetIndexOnShow = true;
        [SerializeField] bool reversed;
        public bool lockNavigation;

        // events
        public event Action OnPreSelect, OnSelected, OnExit, OnOptionChanged;

        // properties
        private MenuCursor Cursor => MenuCursor.Instance;

        public UIInteraction CurrentOption
        {
            get
            {
                // first phase
                if (options == null) Refresh();
                else if (options.Length == 0) Refresh();
                else if (options[optionIndex] == null) Refresh();

                // second phase
                if (options == null || options.Length == 0)
                {
                    Debug.LogWarning("There's no UIInteraction inside this menu");
                    return null;
                }

                try { return options[optionIndex]; }
                catch (IndexOutOfRangeException) { return null; }
            }
        }

        public int OptionIndex
        {
            get => optionIndex;
            set
            {
                if (options == null) Refresh();
                value = Mathf.Clamp(value, 0, options.Length - 1);
                if (value == optionIndex) return;
                ScrollOnNavigate(value);

                if (optionIndex >= 0 && optionIndex < options.Length) CurrentOption?.Exit();
                optionIndex = value;
                CurrentOption?.Enter();
                Cursor.MoveTo(CurrentOption);
                OnOptionChanged?.Invoke();
            }
        }

        public UIInteraction[] Options => options;
        public IEnumerable<int> SelectedIndices => selectedIndices ?? OptionIndex.Enumerate();

        // fields
        internal bool lockScroll;
        private UIInteraction[] options;
        private int optionIndex = 0;
        private ScrollRect scrollRect;
        private int itemPerRow;
        private int maxItemNumInViewport;
        private bool initialized;
        private IEnumerable<int> selectedIndices;

        private void ScrollOnNavigate(int optionIndex)
        {
            if (scrollRect && !lockScroll)
            {
                var padding = maxItemNumInViewport / 2;
                var scrollRate = Mathf.InverseLerp(options.Length - 1 - padding, padding, optionIndex);
                scrollRect.SmoothVertical(scrollRate);
            }
        }

        public override void Show()
        {
            if (!initialized)
            {
                initialized = true;
                Refresh();
            }
            base.Show();
            if (resetIndexOnShow)
            {
                OptionIndex = 0;
                selectedIndices = null;
            }
            if (scrollRect) scrollRect.enabled = true;
        }

        public override void Hide()
        {
            base.Hide();
            if (scrollRect) scrollRect.enabled = false; // prevent content flying while scale = 0
        }

        public override void Focus()
        {
            base.Focus();
            Cursor.Visible = showCursor;
            CurrentOption?.Enter();
            Cursor.MoveToInstantly(CurrentOption);
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
            Cursor.Visible = false;
        }

        public bool Select(bool isLMB)
        {
            if (optionIndex < 0 || isLMB || !CurrentOption || !CurrentOption.Interactable) return false;
            OnPreSelect?.Invoke();
            CurrentOption.Select();
            OnSelected?.Invoke();
            return true;
        }

        public void ToggleAll(bool on)
        {
            foreach (var option in Options) option.Toggle(on);
            selectedIndices = Enumerable.Range(0, options.Length);
            OnOptionChanged?.Invoke();
        }

        public bool Navigate(int index)
        {
            if (lockNavigation) return false;
            var prevOptionIndex = OptionIndex;
            OptionIndex = index;
            return prevOptionIndex != optionIndex;
        }

        public bool Navigate(Vector2 direction)
        {
            if (lockNavigation) return false;
            var prevOptionIndex = OptionIndex;
            if (reversed) direction *= -1;
            switch (scheme)
            {
                case Scheme.Explicit:
                    try
                    {
                        var navigation = CurrentOption.GetComponent<Button>().navigation;
                        OptionIndex = direction switch
                        {
                            { x: 0, y: -1 } => navigation.selectOnDown.GetComponent<UIInteraction>().index,
                            { x: 0, y: 1 } => navigation.selectOnUp.GetComponent<UIInteraction>().index,
                            { x: -1, y: 0 } => navigation.selectOnLeft.GetComponent<UIInteraction>().index,
                            { x: 1, y: 0 } => navigation.selectOnRight.GetComponent<UIInteraction>().index,
                            _ => throw new NotImplementedException(),
                        };
                    }
                    catch (Exception e) { if (logWarning) Debug.LogWarning(e); }
                    break;
                case Scheme.Grid:
                    OptionIndex += (int)(direction.x - direction.y * itemPerRow);
                    break;
                default:
                    var dir = (scheme == Scheme.Horizontal) ? direction.x : -direction.y;
                    OptionIndex += (int)dir;
                    break;
            }
            return prevOptionIndex != optionIndex;
        }

        protected void Start()
        {
            SetupScrolling();
            Refresh();
        }

        private void SetupScrolling()
        {
            scrollRect = GetComponentInChildren<ScrollRect>() ?? GetComponentInParent<ScrollRect>();
            if (!scrollRect) return;
            CalculateVisibleMaxItemNum();
            if (!ShowAnimation.Visible) scrollRect.enabled = false;

            void CalculateVisibleMaxItemNum()
            {
                var verticalScrolling = scrollRect.vertical;
                var viewSize = verticalScrolling ? scrollRect.viewport.rect.height : scrollRect.viewport.rect.width;

                var layout = scrollRect.content.GetComponentInChildren<LayoutGroup>();
                var spacing = layout switch
                {
                    HorizontalOrVerticalLayoutGroup hvLayout => hvLayout.spacing,
                    GridLayoutGroup gridLayout => (verticalScrolling) ? gridLayout.spacing.y : gridLayout.spacing.x,
                    _ => 0
                };

                if (layout.transform.childCount == 0) return;
                var itemRectTransform = layout.transform.GetChild(0).GetComponent<RectTransform>();
                var itemSize = (verticalScrolling) ? itemRectTransform.rect.height : itemRectTransform.rect.width;
                maxItemNumInViewport = (int)((viewSize - spacing) / (itemSize + spacing));
            }
        }

        public void Refresh()
        {
            SetupOptions();
            CalculateItemPerRow();
        }

        private void SetupOptions()
        {
            options = GetComponentsInChildren<UIInteraction>();
            options = options.Where(option => option.GetComponentInParent<Menu>() == this).ToArray();
            for (int i = 0; i < options.Length; i++)
            {
                UIInteraction option = options[i];
                option.ui ??= this;
                option.index = i;
                option.Exit();
            }
        }

        private void CalculateItemPerRow()
        {
            if (scheme != Scheme.Grid) return;
            var grid = GetComponentInChildren<GridLayoutGroup>();
            switch (grid.constraint)
            {
                case GridLayoutGroup.Constraint.Flexible:
                    StartCoroutine(WaitForGridLayoutResize(grid, () =>
                    {
                        var hPadding = grid.padding.left + grid.padding.right;
                        itemPerRow = (int)((grid.preferredWidth - hPadding + grid.spacing.x) / (grid.cellSize.x + grid.spacing.x));
                    }));
                    break;
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    itemPerRow = grid.constraintCount;
                    break;
                case GridLayoutGroup.Constraint.FixedRowCount:
                    itemPerRow = options.Length / grid.constraintCount;
                    break;
                default:
                    break;
            }

            IEnumerator WaitForGridLayoutResize(GridLayoutGroup grid, Action callback)
            {
                yield return new WaitUntil(() => grid.preferredWidth != 0);
                callback?.Invoke();
            }
        }

        public bool CanExit() => canGoBack;

        void IExitListener.OnExit() => OnExit?.Invoke();
    }
}