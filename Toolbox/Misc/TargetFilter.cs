using System;
using System.Collections.Generic;
using System.Linq;

using NaughtyAttributes;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Target Filter")]
public class TargetFilter : ScriptableObject
{
    [SerializeReference, NewButton] List<Filter> filters;
    public bool Check(GameObject gameObject) => filters.All(filter => filter.Check(gameObject));
    public bool Check(BaseCollider collider) => Check(collider.Transform.gameObject);
}

[Serializable]
public abstract class Filter : AutoLabeled
{
    public abstract bool Check(GameObject gameObject);
}

public class TagFilter : Filter
{
    [SerializeField, Tag] string tag;
    public override bool Check(GameObject gameObject) => string.IsNullOrEmpty(tag) || gameObject.CompareTag(tag);
}

public class LayerFilter : Filter
{
    [SerializeField] LayerMask layerMask;
    public override bool Check(GameObject gameObject) => layerMask.Contains(gameObject.layer);
}

public class ComponentFilter : Filter
{
#if UNITY_EDITOR
    [SerializeField, OnValueChanged(nameof(UpdateTypeName))] MonoScript type;
    private void UpdateTypeName() => typeName = type.name;
#endif
    [SerializeField] string typeName;


    [SerializeField] bool includeChildren;
    public override bool Check(GameObject gameObject)
    {
        var type = Type.GetType(typeName);
        if (includeChildren) return gameObject.GetComponentInChildren(type);
        return gameObject.GetComponent(type);
    }
}