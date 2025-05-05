using System;

using UnityEngine;

namespace SkillSystem
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EffectIdAttribute : Attribute
    {
        public readonly string id;
        public EffectIdAttribute(string id) => this.id = id;
    }

    public class EffectIdDropdownAttribute : PropertyAttribute { }
}
