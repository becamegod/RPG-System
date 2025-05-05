using System;
using System.Collections.Generic;
using System.Linq;

using NaughtyAttributes;

using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public class EffectAndParamList
    {
        [ChangeCheck]
        [AllowNesting, OnValueChanged(nameof(OnAssetChanged))]
        [SerializeField] EffectAsset asset;
        public EffectAsset Asset => asset;

        [SerializeReference] List<EffectParam> parameters;
        public IReadOnlyCollection<EffectParam> Parameters => parameters;

        public void OnAssetChanged() => parameters = asset.DefaultParameters.ToList();
    }
    public class ChangeCheckAttribute : PropertyAttribute { }

    [Serializable]
    public abstract class EffectParam : Info, ICloneable
    {
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class ChanceParam : EffectParam
    {
        [SerializeField] float chance = 1;
        public float Chance => chance;
    }

    public class DurationParam : EffectParam
    {
        public const int DefaultValue = 0;
        [SerializeField] int duration = DefaultValue;
        public int Duration => duration;
    }

    public class ApplyToUserParam : EffectParam
    {
        public const bool DefaultValue = true;
        [SerializeField] bool applyToUser = DefaultValue;
        public bool ApplyToUser => applyToUser;
    }

    public class ScaleParam : EffectParam
    {
        public const float DefaultValue = 1;
        [SerializeField] float scale = DefaultValue;
        public float Scale => scale;
    }

    public class DeltaParam : EffectParam
    {
        public const float DefaultValue = 0;
        [SerializeField] float delta = DefaultValue;
        public float Delta => delta;
    }
}
