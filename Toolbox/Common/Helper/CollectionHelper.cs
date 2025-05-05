using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Random = UnityEngine.Random;

public static class CollectionHelper
{
    public static T RandomElement<T>(this IEnumerable<T> enumerable)
    {
        int index = Random.Range(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }

    public static IEnumerable<T> RandomElements<T>(this IEnumerable<T> enumerable, int count)
    {
        return enumerable.OrderBy(x => Random.value).Take(count);
    }

    /// <summary>
    /// Shuffle the list in place using the Fisher-Yates method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int pos = Random.Range(i, list.Count);
            var temp = list[i];
            list[i] = list[pos];
            list[pos] = temp;
        }
    }

    public static IEnumerable<T> Clone<T>(this IList<T> list) where T : ICloneable
    {
        return list.Select(item => (T)item.Clone());
    }

    public static int Replace(this IList source, object oldValue, object newValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var index = source.IndexOf(oldValue);
        if (index != -1) source[index] = newValue;
        return index;
    }

    public static int Replace<T>(this IList<T> source, T oldValue, T newValue) => ((IList)source).Replace(oldValue, newValue);

    public static void ReplaceAll<T>(this IList<T> source, T oldValue, T newValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var index = -1;
        do
        {
            index = source.IndexOf(oldValue);
            if (index != -1) source[index] = newValue;
        } 
        while (index != -1);
    }


    public static IEnumerable<T> Replace<T>(this IEnumerable<T> source, T oldValue, T newValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        return source.Select(x => EqualityComparer<T>.Default.Equals(x, oldValue) ? newValue : x);
    }

    public static IEnumerable<T> With<T>(this IEnumerable<T> source, T value)
    {
        return source.Concat(Enumerable.Repeat(value, 1));
    }

    public static IEnumerable<T> Enumerate<T>(this T item)
    {
        yield return item;
    }
}
