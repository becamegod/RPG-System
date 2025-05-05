using SkillSystem;
using System.Collections.Generic;

using UnityEngine;

public class TurnActionHandler : MonoBehaviour
{
    [SerializeField] EffectResultHandler effectResultHandler;

    public void ExecuteTurnAction(BattleCharacter character, TurnAction action)
    {
        character.AnimateAction(action);

        character.CharacterAnimation.OnHit += UseSkill;
        void UseSkill()
        {
            character.CharacterAnimation.OnHit -= UseSkill;

            // use skill on targets
            foreach (var target in action.targets)
            {
                var results = character.BattleSubject.UseSkill(action.skill, target);
                effectResultHandler.HandleResults(target, results);

                // play VFX
                var vfx = action.skill.Infos.Get<VisualEffectInfo>()?.VisualEffect;
                if (vfx) VisualEffectPools.Play(vfx, target.Center.position);
            }
        }
    }
}
