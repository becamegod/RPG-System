using System;

using UnityEngine;

[CreateAssetMenu(menuName = "Parameter")]
public class Parameter : ScriptableObject
{
    public float value;
    public float defaultValue;

    public static implicit operator float(Parameter x) => x.value;
}
