using System.Collections.Generic;
using System.Linq;

using NaughtyAttributes;

using UnityEngine;

public class RadiusTargetDetector : BaseTargetDetector
{
    [SerializeField] LayerMask layerFilter;
    [SerializeField, Tag] string tagFilter;

    [Header("Target condition")]
    [SerializeField] bool closest = true;
    [SerializeField] bool withinView;

    private List<BaseCollider> targets;
    public IReadOnlyList<BaseCollider> Targets => targets;

    public override BaseCollider Target
    {
        get
        {
            var targets = withinView ? GetTargetsWithinView() : this.targets;
            if (closest) return GetClosestTarget(targets);
            return targets.First();
        }
    }

    private BaseCollider GetClosestTarget(IEnumerable<BaseCollider> targets)
    {
        if (targets.Count() == 0) return null;
        var targetDistances = targets.Select(target => new { target, distance = Vector3.Distance(transform.position, target.Transform.position) });
        var closest = targetDistances.Aggregate((closest, target) => target.distance < closest.distance ? target : closest);
        return closest.target;
    }

    public IEnumerable<BaseCollider> GetTargetsWithinView() => targets.Where(target => Helper.IsWithinView(target.Transform.position));

    private void Awake() => targets = new();

    private bool CheckTarget(GameObject gameObject)
    {
        var tagCheck = string.IsNullOrEmpty(tagFilter) || gameObject.CompareTag(tagFilter);
        var layerCheck = layerFilter.Contains(gameObject.layer);
        return tagCheck && layerCheck;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTarget(other.gameObject)) targets.Add(new(other));
    }

    private void OnTriggerExit(Collider other) => targets.Remove(new(other));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckTarget(collision.gameObject)) targets.Add(new(collision));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(new(collision));
        //foreach (var target in targets)
        //{
        //    if (target.transform == collision.transform)
        //    {
        //        targets.Remove(target);
        //        break;
        //    }
        //}
    }
}

public abstract class BaseTargetDetector : MonoBehaviour
{
    public abstract BaseCollider Target { get; }
}