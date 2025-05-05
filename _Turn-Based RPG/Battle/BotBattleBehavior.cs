using SkillSystem;

using UnityEngine;

public class BotBattleBehavior : BattleBehavior
{
    [SerializeField] SkillDefinition defaultAttack;

    public override void Execute(Context context)
    {
        var character = context.character;
        var skills = character.InfoSubject.Info.Get<SkillsInfo>().Skills;
        ExecuteAction(new(skills.With(defaultAttack).RandomElement(), new[] { context.enemies[0] }));
    }
}