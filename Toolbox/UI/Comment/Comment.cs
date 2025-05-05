using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Comment : MonoBehaviour
{
    [SerializeField] bool playOnAwake;
    [SerializeField] string[] wordPool = { "amazing", "great job", "nice hand" };
    [SerializeField] float showDuration = 2;

    [Header("Easing")]
    [SerializeField] float easeInDuration = 1;
    [SerializeField] Ease easeIn = Ease.OutBounce;
    [SerializeField] float easeOutDuration = .5f;
    [SerializeField] Ease easeOut = Ease.OutQuint;

    private Text text;
    private Outline outline;
    public static Comment Instance;
    private void Awake()
    {
        Instance = this;
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();
        transform.localScale = Vector3.zero;
        if (playOnAwake) ShowRandom();
    }

    public void ShowRandom() => Show(wordPool[Random.Range(0, wordPool.Length)]);

    public void Show(string cmt)
    {
        text.text = cmt;
        transform.localScale = Vector3.zero;
        transform.DOScale(1, easeInDuration).SetEase(easeIn);
        transform.DOScale(0, easeOutDuration).SetDelay(showDuration).SetEase(easeOut);
    }

    public void ShowWithColor(string cmt, Color color)
    {
        outline.effectColor = color;
        Show(cmt);
    }

    public void Hide(float duration = -1)
    {
        if (duration == -1) duration = easeOutDuration;
        transform.DOKill();
        transform.DOScale(0, duration).SetEase(easeOut);
    }
}
