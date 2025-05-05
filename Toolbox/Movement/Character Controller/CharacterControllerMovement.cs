using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : BaseMovement
{
    enum Axis
    {
        Y, Z
    }

    [SerializeField] Axis verticalAxis;
    [SerializeField] Transform axis;

    // fields
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.enabled = true;
    }

    public override Vector3 GetMotion(Vector2 input, bool isRunning = false)
    {
        // input
        var hinput = input.x;
        var vinput = input.y;
        var direction = verticalAxis switch
        {
            Axis.Y => new Vector3(hinput, vinput, 0),
            Axis.Z => new Vector3(hinput, 0, vinput),
            _ => throw new NotImplementedException(),
        };

        // convert axis
        if (axis) direction = axis.forward * direction.y + axis.right * direction.x;

        // move
        var speedMultiplier = isRunning ? runSpeedMultiplier : 1;
        var motion = Speed * speedMultiplier * Time.deltaTime * direction;
        return motion;
    }

    public override void Move(Vector3 motion)
    {
        controller.Move(motion);
        base.Move(motion);
    }
}