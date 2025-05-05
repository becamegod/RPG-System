using UnityEngine;

public class GenericMotion : MonoBehaviour
{
    private Rigidbody rb;
    private Rigidbody2D rb2d;
    private CharacterController controller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<CharacterController>();
    }

    public void Move(Vector3 motion)
    {
        if (rb) rb.linearVelocity = motion;
        else if (rb2d) rb2d.linearVelocity = motion;
        else if (controller) controller.Move(motion);
    }
}
