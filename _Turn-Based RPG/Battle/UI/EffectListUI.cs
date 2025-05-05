using System.Collections.Generic;

using SkillSystem;

using UnityEngine;

public class EffectListUI : MonoBehaviour
{
    [SerializeField] BattleCharacter character;
    [SerializeField] ItemInfoUI prefab;

    Dictionary<LingeringEffect, UIAnimation> uiMap;
    ObservableList<LingeringEffect> Effects => character.BattleSubject.Effects;

    private void Reset() => character = GetComponentInParent<BattleCharacter>();

    private void Awake()
    {
        uiMap = new();
        transform.DestroyChildren();
    }

    private void OnEnable()
    {
        Effects.OnAdded += OnEffectAdded;
        Effects.OnRemoved += OnEffectRemoved;
    }

    private void OnDisable()
    {
        Effects.OnAdded -= OnEffectAdded;
        Effects.OnRemoved -= OnEffectRemoved;
    }

    private void OnEffectAdded(LingeringEffect effect)
    {
        if (effect.Time == 0) return;
        var itemInfoUI = Instantiate(prefab, transform);
        var info = EffectInfoMap.Instance.GetInfo(effect.type);
        itemInfoUI.UpdateInfo(info);

        uiMap[effect] = itemInfoUI.GetComponent<UIAnimation>();
        uiMap[effect].Show();
    }

    private void OnEffectRemoved(LingeringEffect effect)
    {
        if (effect.Time == 0) return;
        var ui = uiMap[effect];
        ui.Hide();
        ui.onHidden += () => Destroy(ui.gameObject);
    }
}
