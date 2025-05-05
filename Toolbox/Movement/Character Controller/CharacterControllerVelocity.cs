using UnityEngine;

public class CharacterControllerVelocity : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    public CharacterController Controller => controller;

    [ReadOnly] public Vector3 value;

    private void LateUpdate() => controller.Move(value * Time.deltaTime);
}