using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NaughtyAttributes;

using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(menuName = Global.AssetMenu + "Effect Info Map")]
    public class EffectInfoMap : ScriptableObjectWithLifecycle
    {
        [Serializable]
        public class Entry
        {
            // identify type
            enum IdentifyType { TypeName, Id }
            [SerializeField] IdentifyType identifyType;

            // type name
            [ShowIf(nameof(identifyType), IdentifyType.TypeName)]
            [SerializeField, TypeDropdown(typeof(LeanEffect))] string effect;
            public Type EffectType => Type.GetType(effect);

            // effect id
            [ShowIf(nameof(identifyType), IdentifyType.Id)]
            [SerializeField, EffectIdDropdown] string id;

            // info reference
            [SerializeField, Expandable] EffectInfoReference info;
            public EffectInfo Info => info.Info;

            // key
            public string Key => identifyType switch
            {
                IdentifyType.TypeName => effect,
                IdentifyType.Id => id,
                _ => null,
            };
        }
        public List<Entry> entries;

        Dictionary<string, EffectInfo> map;
        public static EffectInfoMap Instance { get; private set; }

        public override void OnStart()
        {
            Instance = this;
            map = entries.ToDictionary(entry => entry.Key, entry => entry.Info);
        }

        public override void OnEnd() => Instance = null;

        //public EffectInfo GetInfo(SkillEffect effect) => map[effect.GetType()];
        public EffectInfo GetInfo(IEffect effect)
        {
            var type = effect.GetType();
            var id = type.GetCustomAttribute<EffectIdAttribute>()?.id ?? type.FullName;
            return map.TryGetValue(id, out var info) ? info : null;
        }
        public EffectInfo GetInfo(string effectId) => map[effectId];
        public EffectInfo GetInfo(SimpleEffect effect) => map[effect.GetType().GetCustomAttribute<EffectIdAttribute>().id];
        //public EffectInfo GetInfo(LeanEffect effect) => map[effect.GetType()];
        //public T GetInfo<T>() where T : EffectInfo => map[typeof(T)] as T;
    }
}
