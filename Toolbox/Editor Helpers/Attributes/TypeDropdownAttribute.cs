using System;
using System.Linq;

using UnityEngine;

public class TypeDropdownAttribute : PropertyAttribute
{
    public readonly string[] typeNames;
    public readonly string[] fullTypeNames;
    public TypeDropdownAttribute(Type type)
    {
        var types = Helper.GetAllTypesDerivedFrom(type);
        typeNames = types.Select(type => type.Name).ToArray();
        fullTypeNames = types.Select(type => type.FullName).ToArray();
    }
}
