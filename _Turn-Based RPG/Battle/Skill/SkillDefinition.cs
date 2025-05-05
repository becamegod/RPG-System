using System;
using System.Collections.Generic;

using UnityEngine;

namespace SkillSystem
{
    static class Global
    {
        public const string AssetMenu = "Skill system/";
    }

    [CreateAssetMenu(menuName = Global.AssetMenu + "Skill")]
    public class SkillDefinition : ScriptableObject
    {
        [SerializeReference] List<Info> infos;
        public IReadOnlyCollection<Info> Infos => infos;

        // version 2
        [HideInInspector]
        [SerializeReference, NewButton] List<SkillEffect> effectDefinitions;
        public IReadOnlyCollection<SkillEffect> Effects => effectDefinitions;

        // version 3
        [HideInInspector]
        [SerializeField] List<TurnEffectDefinition> turnEffectDefinitions;
        public IReadOnlyCollection<TurnEffectDefinition> TurnEffectDefinitions => turnEffectDefinitions;

        // version 4
        [SerializeField] List<EffectAndParamList> effectParams;
        public IReadOnlyCollection<EffectAndParamList> EffectParams => effectParams;
    }

    public class SkillsInfo : Info
    {
        [SerializeField] List<SkillDefinition> skills = new();
        public IReadOnlyList<SkillDefinition> Skills => skills;
    }

    public class TargetSideInfo : Info
    {
        public enum TargetSide { Enemy, Ally, Self }
        [SerializeField] TargetSide side;
        public TargetSide Side => side;
    }

    public class TargetTypeInfo : Info
    {
        public enum TargetType { Single, All }
        [SerializeField] TargetType type;
        public TargetType Type => type;
    }
}
