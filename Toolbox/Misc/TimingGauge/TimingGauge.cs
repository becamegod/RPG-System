using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimingGauge : MonoBehaviour
{
    [Serializable]
    public class Region
    {
        public float fromAngle;
        public float rate;
        public Color color;
        internal Image image;
    }

    public float angleSpread = 62;
    [SerializeField] float anglePadding = 5;
    [SerializeField] RectTransform pivot;
    [SerializeField] float halfCycleDuration = 1;
    [SerializeField] Ease ease;
    [SerializeField] float transitionDuration = .5f;
    public Region[] regions;

    [SerializeField] Image regionPrefab;
    [SerializeField] Transform regionParent;

    [SerializeField] bool useLabel;
    [ShowWhen("useLabel", true)][SerializeField] Transform labelPrefab;
    [ShowWhen("useLabel", true)][SerializeField] RectTransform radiusMin;
    [ShowWhen("useLabel", true)][SerializeField] RectTransform radiusMax;

    [SerializeField] bool flashOnStop;
    [ShowWhen("flashOnStop", true)][SerializeField] int flashCount = 3;
    [ShowWhen("flashOnStop", true)][SerializeField] float flashBrightness = 1;
    [ShowWhen("flashOnStop", true)][SerializeField] float flashInterval = .1f;

    private TweenerCore<Quaternion, Vector3, QuaternionOptions> motion;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GenerateRegions();
    }

    private void Start()
    {
        motion = pivot.DOLocalRotate(new Vector3(0, 0, -angleSpread + anglePadding), halfCycleDuration)
            .From(new Vector3(0, 0, angleSpread - anglePadding))
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void GenerateRegions()
    {
        for (int i = 0; i < regions.Length; i++)
        {
            // calculate angle
            Region region = regions[i];
            float spread;
            float fromAngle = region.fromAngle;
            if (i == regions.Length - 1) spread = (90 + angleSpread) - fromAngle;
            else spread = regions[i + 1].fromAngle - fromAngle;
            var normalizeAngle = spread / 360;

            // rotate & fill
            var image = Instantiate(regionPrefab, regionParent);
            region.image = image;
            image.fillAmount = normalizeAngle;
            image.transform.Rotate(0, 0, -fromAngle);

            // clone material
            if (flashOnStop) image.material = new Material(image.material);

            // color
            region.color.a = 1;
            image.color = region.color;

            // label position
            if (useLabel)
            {
                var label = Instantiate(labelPrefab, image.transform);
                //var radiusOffset = pointer.parent.GetComponent<RectTransform>().rect.width / 2 * pointer.parent.localScale.x;
                //var radius = image.GetComponent<RectTransform>().rect.width / 2 * image.transform.localScale.x;
                //var labelOffset = (radius - radiusOffset) / 2 + radiusOffset;

                var labelRT = label.GetComponent<RectTransform>();
                var labelOffset = (radiusMax.position - radiusMin.position).magnitude / 2;
                var radAngle = Mathf.Deg2Rad * (180 - (spread / 2));
                var pos = labelRT.anchoredPosition;
                pos.x += labelOffset * Mathf.Cos(radAngle);
                pos.y += labelOffset * Mathf.Sin(radAngle);
                labelRT.anchoredPosition = pos;
                label.Rotate(0, 0, fromAngle);
                label.Rotate(0, 0, 180 - (fromAngle + spread / 2 + 90));

                // label text
                float modifier = region.rate;
                var text = "x" + modifier.ToString();
                /*region.label = label.GetComponent<Text>();
                region.label.text = text.ToUpper();*/
            }
        }
    }

    public int CurrentIndex
    {
        get
        {
            var angle = 180 - (90 + pivot.eulerAngles.z);
            angle = (angle % 360 + 360) % 360;
            for (int i = 0; i < regions.Length - 1; i++) if (angle < regions[i + 1].fromAngle) return i;
            return regions.Length - 1;
        }
    }

    public Region CurrentRegion => regions[CurrentIndex];

    public float CurrentRate => CurrentRegion.rate;

    internal void Pause()
    {
        motion.Pause();
    }

    internal void Restart()
    {
        motion.Restart();
    }

    internal void Flash(Action endAction = null)
    {
        if (!flashOnStop)
        {
            Debug.LogWarning("Tried to call Flash on a flash disabled TimingGauge (flashOnStop = false)");
            return;
        }
        StartCoroutine(Flashing(endAction));
    }

    private IEnumerator Flashing(Action endAction)
    {
        Material currentMaterial = CurrentRegion.image.materialForRendering;
        for (int i = 0; i < flashCount; i++)
        {
            currentMaterial.SetFloat("_Brightness", flashBrightness);
            yield return new WaitForSeconds(flashInterval);
            currentMaterial.SetFloat("_Brightness", 0);
            yield return new WaitForSeconds(flashInterval);
        }
        endAction?.Invoke();
    }

    internal void Hide() => canvasGroup.DOFade(0, transitionDuration);

    internal void Show() => canvasGroup.DOFade(1, transitionDuration);
}
