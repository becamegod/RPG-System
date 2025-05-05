using DG.Tweening;

using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] Easing moveEasing;
    [SerializeField] Easing returnEasing;

    private Vector3 originalPos;
    private Bounds bounds;

    private void Awake()
    {
        originalPos = transform.position;
        bounds = GetComponentInChildren<Renderer>().bounds;
    }

    public void Move(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;
        var myThickness = (bounds.ClosestPoint(destination) - transform.position).magnitude;
        destination -= direction * myThickness;

        transform.DOMove(destination, moveEasing.Duration).SetEase(moveEasing.Ease);
    }

    internal void Return()
    {
        transform.DOMove(originalPos, returnEasing.Duration).SetEase(returnEasing.Ease);
    }
}
