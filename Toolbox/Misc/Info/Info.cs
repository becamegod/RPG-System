using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.VFX;

public static class InfoExtension
{
    public static T Get<T>(this IReadOnlyCollection<Info> infos) where T : Info
    {
        try { return infos.First(info => info.GetType() == typeof(T)) as T; }
        catch (InvalidOperationException) { return null; }
    }

    public static Info Get(this IReadOnlyCollection<Info> infos, Type type)
    {
        try { return infos.First(info => info.GetType() == type); }
        catch (InvalidOperationException) { return null; }
    }
}

[Serializable]
public abstract class Info : AutoLabeled, INewButton
{
}

public class SpriteInfo : Info
{
    [SerializeField] Sprite sprite;
    public Sprite Sprite => sprite;
}

public class VisualEffectInfo : Info
{
    [SerializeField] VisualEffect visualEffect;
    public VisualEffect VisualEffect => visualEffect;
}

public class AnimationClipInfo : Info
{
    [SerializeField] AnimationClip clip;
    public AnimationClip Clip => clip;
}

// unsorted
public class RangeTypeInfo : Info
{
    public enum RangeType { Melee, Ranged }
    [SerializeField] RangeType type;
    public RangeType Type => type;
}

public class DurationInfo : Info
{
    [SerializeField] float duration;
    public float Duration => duration;
    public DurationInfo(float duration) => this.duration = duration;
}
