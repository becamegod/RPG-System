using System;

using UnityEngine;

namespace SkillSystem
{
    [Serializable]
    public class EffectAndParams
    {
        [SerializeField] EffectAsset asset;
        [SerializeReference] EffectParams param;
    }

    [Serializable]
    public abstract class EffectParams
    {
        [SerializeField] float successRate = 1;
        [SerializeField] int duration;
        [SerializeField] bool applyToUser;
    }

    public class DeltaRateParams : EffectParams
    {
        [SerializeField] float deltaRate;
    }
}
