using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TimingGauge))]
public class RegionGenerator : MonoBehaviour
{
    [Serializable]
    class RegionRank
    {
        public float rate;
        public Color color;
    }

    [SerializeField] List<RegionRank> regionRanks;
    [SerializeField] bool isHighestCenter;

    [ContextMenu("Generate")]
    public void Generate()
    {
        var ranks = new List<RegionRank>(regionRanks);
        if (isHighestCenter)
        {
            var addition = ranks.Skip(1).ToArray();
            ranks.Reverse();
            ranks.AddRange(addition);
        }

        var timingGauge = GetComponent<TimingGauge>();
        var totalSpread = timingGauge.angleSpread * 2;
        var angleDelta = totalSpread / ranks.Count;
        var angle = 90 - timingGauge.angleSpread;

        var x = ranks.Select(rank =>
        {
            var region = new TimingGauge.Region();
            region.fromAngle = angle;
            region.rate = rank.rate;
            region.color = rank.color;
            angle += angleDelta;
            return region;
        }).ToArray();
        timingGauge.regions = x;
    }
}
