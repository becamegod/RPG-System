using UnityEngine;

public class TurnOnAtStart : MonoBehaviour
{
    [SerializeField] Behaviour component;

    private Behaviour prevComponent;

    //private void OnValidate()
    //{
    //    if (Application.isPlaying) return;
    //    if (prevComponent) prevComponent.enabled = true;
    //    if (component) component.enabled = false;
    //    prevComponent = component;
    //}

    private void Awake()
    {
        if (!component) return;
        component.enabled = true;
    }
}
