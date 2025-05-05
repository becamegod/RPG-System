using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BounceButtonAnimation : MonoBehaviour
{
    [SerializeField] float bounceStrength = .2f;
    [SerializeField] float bounceDuration = .2f;
    [SerializeField] float elasticity = .5f;
    [SerializeField] int vibrato = 5;
    private Vector3 bounceVector;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        bounceVector = new Vector3(bounceStrength, bounceStrength, bounceStrength);
    }

    private void OnClick()
    {
        transform.DOPunchScale(bounceVector, bounceDuration, vibrato, elasticity);
    }
}
