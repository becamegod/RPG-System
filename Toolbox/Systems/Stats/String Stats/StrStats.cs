using System;

using StatsSystem;

using UnityEngine;

[CreateAssetMenu(menuName = Global.AssetMenu + "String Stats")]
public class SStats : BaseStats<SStat>
{
}

[Serializable]
public class SStat : BaseStat<string>
{
    public SStat(BaseStat<string> source) : base(source)
    {
    }

    public override string Name => name;

    public override object Clone() => new SStat(this);
}