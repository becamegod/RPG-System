using System.Collections.Generic;

using SkillSystem;

using UnityEngine;

public class EffectPopupUI : MonoBehaviour
{
    [SerializeField] BattleCharacter character;
    [SerializeField] TextPopup textPopupPrefab;
    ObservableList<LingeringEffect> Effects => character.BattleSubject.Effects;

    private void Reset() => character = GetComponentInParent<BattleCharacter>();

    private void OnEnable() => Effects.OnAdded += OnEffectAdded;

    private void OnDisable() => Effects.OnAdded -= OnEffectAdded;

    private void OnEffectAdded(LingeringEffect effect)
    {
        var info = EffectInfoMap.Instance.GetInfo(effect.type);
        if (info is null) return;
        TextPopupPools.Popup(textPopupPrefab, character.Center.position, info.PopupText);
    }
}
