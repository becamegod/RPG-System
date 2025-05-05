using System;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;

using ScriptableEffect;

using SkillSystem;

using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    // animation
    [SerializeField] CharacterAnimation characterAnimation;
    public CharacterAnimation CharacterAnimation => characterAnimation;

    // movement
    [SerializeField] CharacterMovement characterMovement;
    public CharacterMovement CharacterMovement => characterMovement;

    // hp bar
    [SerializeField] ReferencedStatBar hpBar;

    // center
    [SerializeField] Transform center;
    public Transform Center => center;

    // info
    [SerializeField] InfoSubject infoSubject;
    public InfoSubject InfoSubject => infoSubject;

    // stats
    [SerializeField] RStats stats;
    public RStats Stats => stats;
    public RStat Health => stats["Health"];
    public RStat Strength => stats["Strength"];
    public RStat Magic => stats["Magic"];

    // battle subject
    [NaughtyAttributes.ReadOnly, SerializeField] BattleSubject battleSubject;
    public BattleSubject BattleSubject => battleSubject;

    //IEnumerable<Transform> targets;
    public IEnumerable<Transform> Targets;// => targets;

    // temp
    [SerializeField] VisualEffectEvent hurtFx;

    private Vector3 targetPosition;

    private void Awake()
    {
        stats.Init();
        hpBar.stats = stats;

        battleSubject = new(stats);
        SetupBattleSubjectInfo();
        SetupPassive();
        SetupDamageModifiers();
    }

    private void SetupDamageModifiers()
    {
        BattleSubject.damageModifiers.Add(new CriticalDamageModifier(BattleSubject));
    }

    private void SetupBattleSubjectInfo()
    {
        var infos = InfoSubject.Info.Get<BattleSubjectInfo>()?.infos;
        if (infos is null) return;
        battleSubject.SetMofifierInfo(infos);
    }

    private void SetupPassive()
    {
        var passives = InfoSubject.Info.Get<PassiveEffectListInfo>()?.Passives;
        if (passives is null) return;

        foreach (var passive in passives)
        {
            var context = UsageContextFactory.FromBattleSubject(this, this);
            context.AddParameters(passive.Parameters);
            BattleSubject.AddEffect(passive.Asset.Effect, context);
        }
    }

    private void OnDestroy()
    {
        //RevertPassiveEffects();
        RevertActiveEffects();
    }

    private void RevertActiveEffects()
    {
        var effects = BattleSubject.Effects;
        if (effects is not null) foreach (var effect in effects) effect.OnEffectEnd();
    }

    //private void RevertPassiveEffects()
    //{
    //    var passives = InfoSubject.Info.Get<PassiveEffectListInfo>()?.Passives;
    //    if (passives is null) return;

    //    var context = UsageContextFactory.FromBattleSubject(this, this);
    //    foreach (var passive in passives) passive.Asset.Effect.OnEffectEnd(context);
    //}

    private void Start()
    {
        var enemies = BattleController.Instance.GetEnemies(this);
        LookAt(BattleController.GetSideCenter(enemies));
    }

    public Vector3 Move()
    {
        characterMovement.Move(targetPosition);
        return targetPosition;
    }

    internal void LookAt(Vector3 position) => transform.DOLookAt(position, .5f);

    internal void Hurt(VisualEffectEvent visualEffect = null)
    {
        characterAnimation.Hurt();
        VisualEffectPools.Play(visualEffect ?? hurtFx, center.position);
    }

    public void Animate(IEnumerable<Transform> targets, AnimationClip clip)
    {
        Targets = targets.Select(target => target.transform);
        CharacterAnimation.Animate(clip);
    }

    public void AnimateAction(TurnAction action)
    {
        // get targets
        var skill = action.skill;
        var targets = action.targets;
        Targets = targets.Select(target => target.transform);

        // look at group center
        var positions = Targets.Select(target => target.position);
        targetPosition = positions.Average();
        LookAt(targetPosition);

        // play animation
        var clip = skill.Infos.Get<AnimationClipInfo>()?.Clip;
        var ranged = skill.Infos.Get<RangeTypeInfo>()?.Type ?? RangeTypeInfo.RangeType.Melee;
        CharacterAnimation.Animate(clip, ranged);
    }

    #region ghost code
    //internal List<List<EffectResult>> Act(TurnAction action)
    //{
    //    var resultsList = new List<List<EffectResult>>();

    //    // use skill on targets
    //    var skill = action.skill;
    //    var targets = action.targets;
    //    foreach (var target in targets)
    //    {
    //        var results = UseSkill(skill, target);
    //        resultsList.Add(results);
    //    }

    //    // animate
    //    Animate(targets.Select(target => target.transform), skill.Infos.Get<AnimationClipInfo>()?.Clip);

    //    return resultsList;
    //}
    #endregion

    //internal List<EffectResult> UseSkill(SkillDefinition skill, BattleCharacter target)
    //{
    //    return battleSubject.UseSkill(skill, target);
    //    var results = new List<EffectResult>();
    //    foreach (var effect in skill.Effects)
    //    {
    //        var context = new UsageContext(this, effect.ApplyToUser ? this : target);
    //        results.Add(effect.OnFirstApply(context));
    //    }

    //    // play VFX
    //    var vfx = skill.Infos.Get<VisualEffectInfo>()?.VisualEffect;
    //    if (vfx) VisualEffectPools.Play(vfx, target.Center.position);

    //    return results;
    //}

    public static implicit operator BattleSubject(BattleCharacter character) => character.BattleSubject;

    //public List<EffectResult> ProcessTurnStart() => battleSubject.ProcessTurnStart();
    //internal void TakeDamage(int damage) => battleSubject.TakeDamage(damage);
}
