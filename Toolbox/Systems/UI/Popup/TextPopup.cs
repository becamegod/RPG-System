using UnityEngine;

[RequireComponent(typeof(GenericText))]
public class TextPopup : BaseEnterExitEffect
{
    [SerializeField] float liveTime = 1f;

    private GenericText text;
    public GenericText Text => text;

    private void Awake() => text = GetComponent<GenericText>();

    public override void Enter()
    {
        base.Enter();
        Invoke(nameof(Exit), liveTime);
    }
}
