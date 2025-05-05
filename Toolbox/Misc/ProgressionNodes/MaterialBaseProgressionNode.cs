using StateSystem;
using UnityEngine;
using UnityEngine.UI;

public class MaterialBaseProgressionNode : MonoBehaviour, IProgressionNode
{
    [SerializeField] float highlightBrightness = .5f;

    public string text
    {
        get => label.text;
        set => label.text = value;
    }

    private Image image;
    private Text label;
    private State normalState;
    private State highlightState;
    private State disabledState;
    private StateMachine stateMachine;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.material = new Material(image.material);
        label = GetComponentInChildren<Text>();

        InitStates();
    }

    private void InitStates()
    {
        normalState = new State();
        highlightState = new State()
            .SetStart(() => image.material.SetFloat("_Brightness", highlightBrightness))
            .SetEnd(() => image.material.SetFloat("_Brightness", 0));
        disabledState = new State()
            .SetStart(() => image.material.SetFloat("_Saturation", 0))
            .SetEnd(() => image.material.SetFloat("_Saturation", 1));

        stateMachine = new StateMachine(normalState);
    }

    public void Dehighlight()
    {
        stateMachine.State = normalState;
    }

    public void Highlight()
    {
        stateMachine.State = highlightState;
    }

    public void Disable()
    {
        stateMachine.State = disabledState;
    }
}
