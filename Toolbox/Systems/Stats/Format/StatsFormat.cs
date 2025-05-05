using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using UnityEngine;

[CreateAssetMenu(menuName = "Stats System/Format")]
public class StatsFormat : ScriptableObject
{
    [SerializeField] List<string> names;

    public ReadOnlyCollection<string> Names => new(names);

    //public List<string> Names => names;
}

[Serializable]
public class FormattedStats
{
    Dictionary<string, float> valueMap;

    ReadOnlyDictionary<string, float> Values => new(valueMap);

    public FormattedStats(StatsFormat format)
    {
        valueMap = format.Names.ToDictionary(name => name, name => 0f);
    }

    public float this[string name] => Values[name];
}