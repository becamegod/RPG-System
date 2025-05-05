using UnityEngine;

public class BehaviorSet : MonoBehaviour
{
    [SerializeField] Behaviour current;
    public Behaviour Current
    {
        get => current;
        set
        {
            if (value == current) return;
            if (current) current.enabled = false;
            current = value;
            if (current) current.enabled = true;
        }
    }

    private Behaviour temp;

    public void TemporalSet(Behaviour value)
    {
        temp = current;
        Current = value;
    }

    public void Revert()
    {
        if (temp) Current = temp;
    }

    public void ClearTemp() => temp = null;
}
