using System;
using System.Collections.Generic;

using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Effect Asset")]
    public class EffectAsset : ScriptableObject
    {
        [SerializeField] EffectInfo info;
        public EffectInfo Info => info;

        [SerializeReference, NewButton(true)] LeanEffect effect;
        public IEffect Effect => effect;

        // belong to LeanEffect
        [SerializeReference] List<EffectParam> defaultParameters;
        public IReadOnlyCollection<EffectParam> DefaultParameters => defaultParameters;

        // simple effect (won't be used)
        [HideInInspector]
        [SerializeReference, NewButton(true)] SimpleEffect simpleEffect;
        public SimpleEffect SimpleEffect => simpleEffect;
    }
}
