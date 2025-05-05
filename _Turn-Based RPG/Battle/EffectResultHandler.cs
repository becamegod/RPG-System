using System.Collections.Generic;

using SkillSystem;

using UnityEngine;

public class EffectResultHandler : MonoBehaviour
{
    [SerializeField] TextPopup textPopupPrefab;
    [SerializeField] bool log;

    public void HandleResults(BattleCharacter target, List<EffectResult> results)
    {
        foreach (var result in results) HandleResult(target, result);
    }

    public void HandleResult(BattleCharacter target, EffectResult result)
    {
        if (result is DamageResult damageResult)
        {
            if (log) Debug.Log($"Enemy got hit: {target.gameObject.name}");
            target.Health.Value -= damageResult.damage;
            target.Hurt(damageResult.visualEffect);
            //target.BattleSubject.TakeDamage(new( damageResult.damage);
            //TextPopupPools.Popup(textPopupPrefab, target.Center.position, info.PopupText);
            foreach (var modifierResult in damageResult.modifierResults)
            {
                if (modifierResult is CriticalResult) TextPopupPools.Popup(textPopupPrefab, target.Center.position, "Critical");
            }
        }
        else if (result is LingeringEffectResult lingeringEffectResult)
        {
            if (lingeringEffectResult.effect.Time > 0)
            {
                if (log) Debug.Log($"Enemy {target.gameObject.name} got {lingeringEffectResult.effect.type} effect");

                // add to list
                target.BattleSubject.Effects.Add(lingeringEffectResult.effect);

                // popup text
                var info = EffectInfoMap.Instance.GetInfo(lingeringEffectResult.effect.type);
                TextPopupPools.Popup(textPopupPrefab, target.Center.position, info.PopupText);
            }
            else lingeringEffectResult.effect.OnEffectEnd();
        }
        //else if (result is )
    }
}
