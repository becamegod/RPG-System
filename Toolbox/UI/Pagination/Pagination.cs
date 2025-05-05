using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TouchInput))]
public class Pagination : MonoBehaviour
{
    [SerializeField] float transitionDuration = .5f;
    [SerializeField] Ease ease = Ease.OutQuint;
    [SerializeField] Transform pages;

    private RectTransform Rt => pages.GetComponent<RectTransform>();

    private TouchInput input;
    private int currentIndex;

    public int CurrentIndex
    {
        get => currentIndex;
        set
        {
            var newValue = Mathf.Clamp(value, 0, pages.childCount - 1);
            if (currentIndex == newValue) return;
            currentIndex = newValue;

            var child = pages.GetChild(currentIndex);
            pages.DOKill();
            pages.DOLocalMove(pages.GetChild(0).localPosition - child.localPosition, transitionDuration).SetEase(ease);
        }
    }

    private void Awake()
    {
        input = GetComponent<TouchInput>();
        input.onSwipeHorizontal += OnSwipe;
    }

    private void Start()
    {
        SetCellSize();

    }

    private void OnSwipe(float direction)
    {
        CurrentIndex -= (int)direction;
    }

    //[ContextMenu("AutoSizeChildren")]
    //public void AutoSizeChildren()
    //{
    //    var width = GetComponent<RectTransform>().rect.width;
    //    for (int i = 0; i < pages.childCount; i++)
    //    {
    //        var child = pages.GetChild(i);
    //        var rt = child.GetComponent<RectTransform>();
    //        var rect = rt.rect;
    //        rect.width = width;
    //        rt.rect.Set(rect.width / 2, rect.y, rect.width, rect.height);
    //    }
    //}

    [ContextMenu("SetCellSize")]
    public void SetCellSize()
    {
        var grid = pages.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(Rt.rect.width, Rt.rect.height);
    }
}
