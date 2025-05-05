using System;

using StatsSystem;

using UnityEngine;

[CreateAssetMenu(menuName = Global.AssetMenu + "Ref Stats")]
public class RStats : BaseStats<RStat>
{
}

[Serializable]
public class RStat : BaseStat<ScriptableReference>
{
    public RStat(BaseStat<ScriptableReference> source) : base(source)
    {
    }

    public override string Name => name.name;

    public override object Clone() => new RStat(this);
}