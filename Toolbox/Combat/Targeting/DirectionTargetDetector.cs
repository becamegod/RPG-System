using System.Linq;

using UnityEngine;

public class DirectionTargetDetector : BaseTargetDetector
{
    [SerializeField] RadiusTargetDetector radiusTargetDetector;
    [SerializeField] DirectionInput direction;

    public override BaseCollider Target => GetTargetInPlayerDirection();

    public BaseCollider GetTargetInPlayerDirection()
    {
        var targets = radiusTargetDetector.Targets;
        if (targets.Count == 0) return null;

        var targetValues = targets.Select(target => new
        {
            target,
            value = Vector3.Dot(direction.Value, target.bounds.center - transform.position)
        });
        var maxValue = targetValues.Max(targetValue => targetValue.value);
        return targetValues.First(targetValue => targetValue.value == maxValue).target;
    }
}
