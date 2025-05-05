using System;
using UnityEngine;

public abstract class BaseJumper : MonoBehaviour
{
    [SerializeField] protected float jumpForce;
    [SerializeField] protected LayerMask groundMask;

    // props
    public abstract bool IsGrounded { get; }
    public abstract float VelocityY { get; }

    // events
    public event Action OnJumped;

    public virtual void Jump() => OnJumped?.Invoke();
}
