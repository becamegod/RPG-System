using Unity.Cinemachine;

using UnityEngine;

public class CameraFOVAnimation : MonoBehaviour
{
    [SerializeField] CinemachineCamera vcam;
    [SerializeField] float fovTarget;
    [SerializeField] float duration = .5f;

    private float prevFov = -1;

    public float Duration => duration;

    public void Play()
    {
        if (prevFov == -1) prevFov = vcam.Lens.FieldOfView;
        vcam.DOFOV(fovTarget, duration);
    }

    public void Revert() => vcam.Lens.FieldOfView = prevFov;
}
