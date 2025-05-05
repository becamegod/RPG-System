using UnityEngine;

public class CharacterControllerGravity : MonoBehaviour
{
    [SerializeField] CharacterControllerVelocity velocity;
    [SerializeField] float gravityScale = 1;

    Vector3 Gravity => gravityScale * Physics.gravity;

    private void Update()
    {
        if (velocity.Controller.isGrounded)
        {
            var vector3 = Vector3.Scale(velocity.value, Gravity.normalized);
            velocity.value += vector3;
        }

        velocity.value += Gravity * Time.deltaTime;
    }
}