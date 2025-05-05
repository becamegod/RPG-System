using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rigidbody2DMovement : BaseMovement
{
    // fields
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void ReceiveInput(Vector2 input, bool isRunning = false)
    {
        base.ReceiveInput(input, isRunning);

        // input
        var direction = input.normalized;

        // move
        var speedMultiplier = isRunning ? runSpeedMultiplier : 1;
        rb.linearVelocity = Speed * speedMultiplier * direction;
        //rb.position += speed * speedMultiplier * Time.deltaTime * direction;
        //rb.MovePosition(rb.position + speed * speedMultiplier * Time.deltaTime * direction);
    }
}