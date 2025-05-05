using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

using UnityEngine;

using Random = UnityEngine.Random;

public static class Helper
{
    public static void RepeatAction(int repeatCount, Action action)
    {
        for (int i = 0; i < repeatCount; i++) action();
    }

    public static void RepeatAction(int repeatCount, Action<int> action)
    {
        for (int i = 0; i < repeatCount; i++) action(i);
    }

    public static void DelayAction(float delay, Action action)
    {
        new Thread(new ThreadStart(() =>
        {
            Thread.Sleep((int)(delay * 1000));
            action?.Invoke();
        })).Start();
    }

    public static Vector2 GetRandomPoint(this Rect rect) => new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));

    public static bool Chance(float rate) => Random.value < rate;

    public static float Remap(this float value, float valueRangeMin, float valueRangeMax, float newRangeMin, float newRangeMax)
    {
        return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
    }

    public static IEnumerable<Type> GetAllTypesDerivedFrom<T>() => GetAllTypesDerivedFrom(typeof(T));

    public static IEnumerable<Type> GetAllTypesDerivedFrom(Type type, bool scanAllAssemblies = false)
    {
        var assemblies = scanAllAssemblies ? AppDomain.CurrentDomain.GetAssemblies() : type.Assembly.Enumerate();
        return assemblies.SelectMany(assembly => assembly.GetTypes()).Where(t => t.IsSubclassOf(type) && !t.IsAbstract);
    }

    public static string SplitPascalCase(this string str) => Regex.Replace(str, "(\\B[A-Z])", " $1");

    public static bool Contains(this LayerMask mask, int layer)
    {
        return (mask & 1 << layer) != 0;
    }

    public static Quaternion ToRotation2D(this Vector2 delta, float offset = 0)
    {
        var angle = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x) + offset;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static Quaternion ToRotation2D(this Vector3 delta, float offset = 0)
    {
        return ((Vector2)delta).ToRotation2D(offset);
    }

    public static Vector2 Rotate(this Vector2 vector, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * vector;
    }

    public static bool IsWithinView(Vector3 position)
    {
        var viewPos = Camera.main.WorldToViewportPoint(position);
        return viewPos.x is >= 0 and <= 1 && viewPos.y is >= 0 and <= 1 && viewPos.z >= 0;
    }

    public static void ReplaceClip(this Animator animator, AnimationClip baseClip, AnimationClip newClip)
    {
        var overrider = new AnimatorOverrideController(animator.runtimeAnimatorController);

        // copy overrides from old overrider
        if (animator.runtimeAnimatorController is AnimatorOverrideController oldOverrider)
        {
            var oldOverrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            oldOverrider.GetOverrides(oldOverrides);
            overrider.ApplyOverrides(oldOverrides);
        }

        // apply new overrides
        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>
        {
            new(baseClip, newClip)
        };
        overrider.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrider;
    }

    public static Vector3 Average(this IEnumerable<Vector3> vectors)
    {
        Vector3 sum = Vector3.zero;
        foreach (var vector in vectors) sum += vector;
        return sum / vectors.Count();
    }

    public static float ToFloat(this bool value) => value ? 1 : 0;
    public static string ToSuccess(this bool value) => value ? "successful" : "failed";
    public static bool ToBool(this int value) => Convert.ToBoolean(value);
}
