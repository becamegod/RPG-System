using System;
using System.Collections;

using UnityEngine;

public class ShortMovement : MonoBehaviour
{
    [SerializeField] BaseMovement movement;

    [SerializeField] Transform directionReference;
    public Transform DirectionReference
    {
        get => directionReference;
        set => directionReference = value;
    }

    [SerializeField] float speed = 15;
    [SerializeField] float duration = 1;
    [SerializeField] AnimationCurve curve;

    public event Action OnStarted, OnEnded;

    public void Move() => StartCoroutine(MoveCR());

    IEnumerator MoveCR()
    {
        OnStarted?.Invoke();

        var time = 0f;
        var direction = directionReference.forward;

        while (time < duration)
        {
            time += Time.deltaTime;
            var value = curve.Evaluate(time / duration);
            movement.Move(speed * Time.deltaTime * value * direction);
            yield return null;
        }

        OnEnded?.Invoke();
    }
}
