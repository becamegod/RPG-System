using UnityEngine;

namespace StatsSystem
{
    public class StatsOwner : MonoBehaviour
    {
        [SerializeField] RStats stats;
        public RStats Stats => stats;
    }
}
