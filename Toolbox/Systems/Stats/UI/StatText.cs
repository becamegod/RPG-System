using UnityEngine;

public class StatText : MonoBehaviour
{
    [SerializeField] GenericText text;
    [SerializeField] Stat stat;

    private void Start()
    {
        stat.OnChanged += UpdateBar;
        UpdateBar();
    }

    private void OnDestroy()
    {
        stat.OnChanged -= UpdateBar;
    }

    private void UpdateBar() => text.SetText(stat.Value.ToString());
}
