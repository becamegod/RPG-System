using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DJumper : BaseJumper
{
    // props
    public override bool IsGrounded => Physics2D.CircleCast(rb.position, .1f, Vector2.down, 0, groundMask);
    public override float VelocityY => rb.linearVelocity.y;

    // fields
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Jump()
    {
        if (!IsGrounded) return;
        base.Jump();
        rb.AddForce(Vector2.up * jumpForce);
    }
}