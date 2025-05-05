using UnityEngine;

public class TextPopupPools : BasePools<TextPopupPools, TextPopup>
{
    public static void Popup(TextPopup prefab, Vector3 position, string text)
    {
        var popup = Instance.Spawn(prefab, position);
        popup.Text.Text = text;
        popup.Enter();
        popup.OnExited += Despawn;

        void Despawn()
        {
            popup.OnExited -= Despawn;
            Instance.Despawn(prefab, popup);
        }
    }
}