using System;

using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    [SerializeField] float topSpeed = 1;
    [SerializeField] protected float runSpeedMultiplier = 2;
    [SerializeField] bool accelerate;
    [SerializeField, ShowWhen(nameof(accelerate))] float acceleration = .1f;

    // props
    public float RunMultiplier
    {
        get => runSpeedMultiplier;
        set => runSpeedMultiplier = value;
    }
    public float Speed
    {
        get
        {
            return speed;
        }
        set => speed = value;
    }

    // events
    public event Action<Vector2, bool> OnInput;
    public event Action<Vector3> OnMoved;

    // fields
    private float speed;
    private Vector2 inputMomentum;
    private bool isPressing;

    public virtual void ReceiveInput(Vector2 input, bool isRunning = false)
    {
        isPressing = input.magnitude > 0;
        if (isPressing) inputMomentum = input;
        OnInput?.Invoke(input, isRunning);
    }

    private void Update()
    {
        UpdateSpeed();
        Move(GetMotion(inputMomentum * Speed / topSpeed, false));
    }

    private void UpdateSpeed()
    {
        if (isPressing) speed = accelerate ? Mathf.MoveTowards(speed, topSpeed, acceleration) : topSpeed;
        else speed = accelerate ? Mathf.MoveTowards(speed, 0, acceleration) : 0;
    }

    public virtual Vector3 GetMotion(Vector2 input, bool isRunning = false) => Vector3.zero;

    public virtual void Move(Vector3 motion) => OnMoved?.Invoke(motion);
}

interface IMovementEvent
{

}

