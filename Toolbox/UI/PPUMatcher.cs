using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PPUMatcher : MonoBehaviour
{
    [SerializeField] Image target;
    private Image Image => GetComponent<Image>();

    private void Awake() => Match();

    private void Match()
    {
        Image.pixelsPerUnitMultiplier = target.pixelsPerUnitMultiplier;
    }

    private void OnValidate() => Match();

    private void Reset() => Match();
}
