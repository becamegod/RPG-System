using DG.Tweening;
using UnityEngine;

public class DOTweenCapacity : MonoBehaviour
{
    [SerializeField] int tweenersCapacity = 500;
    [SerializeField] int sequencesCapacity = 50;

    private void Awake() => DOTween.SetTweensCapacity(tweenersCapacity, sequencesCapacity);
}
