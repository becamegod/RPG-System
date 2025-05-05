using UnityEngine;

public class InstantAnimation : UIAnimation
{
    [SerializeField] GameObject obj;

    private void Reset() => obj = gameObject;

    public override void Hide()
    {
        base.Hide();
        obj.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        obj.SetActive(true);
    }
}
