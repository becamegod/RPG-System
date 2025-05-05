using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class TextBox : BaseUI, ISelectable, IExitBlocking
    {
        [Header("References")]
        [SerializeField] TextMeshProUGUI bodyText;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] Image avatar;
        [SerializeField] VisibilityParent nextMark;
        [SerializeField] GameObject tail;

        [Header("Tweaking")]
        [SerializeField] float characterInterval = .02f;
        [SerializeField] float commaDelay = .25f;
        [SerializeField] float periodDelay = .5f;
        [SerializeField] float transitionDuration = .5f;
        [SerializeField] float transitionOffset = 10;
        [SerializeField] bool canExit;

        // events
        public event Action OnCharacterAdded;

        // fields
        private bool isFinished;
        private Coroutine runTextCR;
        private WaitForSeconds waitForCharacterInterval;
        private WaitForSeconds waitForCommaDelay;
        private WaitForSeconds waitForPeriodDelay;
        private CanvasScaler canvasScaler;
        private string parsedText;
        private int textLength;

        // props
        public bool IsFinished => isFinished;
        public TMP_StyleSheet StyleSheet => bodyText.styleSheet;

        public string Content
        {
            get => bodyText.text;
            set
            {
                bodyText.text = value;
                bodyText.ForceMeshUpdate();
                parsedText = bodyText.GetParsedText();
                textLength = parsedText.Length;
            }
        }

        public int CharCount
        {
            get => bodyText.maxVisibleCharacters;
            set
            {
                value = Mathf.Clamp(value, 0, textLength);
                bodyText.maxVisibleCharacters = value;
                if (CharCount == textLength) nextMark.Show();
            }
        }

        public string Name
        {
            get => nameText.text;
            set => nameText.text = value;
        }

        public Sprite Avatar
        {
            get => avatar?.sprite;
            set
            {
                if (!avatar || avatar.sprite == value) return;
                avatar.sprite = value;
                if (!value) return;
                avatar.SetNativeSize();
                var center = new Vector2(value.rect.width / 2, value.rect.height / 2);
                var offset = center - value.pivot;
                offset *= canvasScaler.referencePixelsPerUnit / value.pixelsPerUnit;
                avatar.GetComponent<RectTransform>().anchoredPosition = offset;
            }
        }

        public bool TailVisible
        {
            get => tail && tail.activeSelf;
            set
            {
                if (!tail || TailVisible == value) return;
                tail.SetActive(value);
            }
        }

        public bool HaveShownAllText => CharCount == textLength;

        public void RunText()
        {
            isFinished = false;
            runTextCR = StartCoroutine(RunTextCR());

            IEnumerator RunTextCR()
            {
                // init
                CharCount = 0;
                nextMark.Hide();

                // advance
                while (CharCount < textLength)
                {
                    CharCount++;
                    OnCharacterAdded?.Invoke();
                    yield return parsedText[CharCount - 1] switch
                    {
                        ',' => waitForCommaDelay,
                        '.' or '?' or '!' => waitForPeriodDelay,
                        _ => waitForCharacterInterval
                    };
                }
            }
        }

        public bool Select(bool isLMB)
        {
            if (runTextCR == null) return false;
            if (CharCount < textLength) CharCount = textLength;
            else isFinished = true;
            return true;
        }

        public void ClearBody() => bodyText.text = "";

        private new void Awake()
        {
            base.Awake();
            bodyText.text = "";
            waitForCharacterInterval = new WaitForSeconds(characterInterval);
            waitForCommaDelay = new WaitForSeconds(characterInterval + commaDelay);
            waitForPeriodDelay = new WaitForSeconds(characterInterval + periodDelay);
            canvasScaler = MaskUtilities.FindRootSortOverrideCanvas(transform).GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            if (ShowAnimation) ShowAnimation.onPreShow += () =>
            {
                nextMark.Hide();

                if (!avatar) return;
                var t = avatar.transform;
                t.DOLocalMoveX(t.localPosition.x, transitionDuration).From(t.localPosition.AddX(transitionOffset));
                avatar.DOFade(1, transitionDuration).From(0);
            };
        }

        public override void LoseFocus()
        {
            base.LoseFocus();
            runTextCR = null;
        }

        public bool CanExit() => canExit;
    }
}
