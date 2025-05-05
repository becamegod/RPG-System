using UnityEngine;

[RequireComponent(typeof(GenericText))]
public class NameText : MonoBehaviour
{
    [SerializeField] GenericText text;
    [SerializeField] IName subject;

    private void Reset()
    {
        text ??= GetComponent<GenericText>();
        subject ??= GetComponentInParent<IName>();
    }

    private void Awake()
    {
        Reset();
        text.Text = subject.Name;
    }
}