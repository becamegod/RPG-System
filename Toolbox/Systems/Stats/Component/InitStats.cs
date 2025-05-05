using UnityEngine;

namespace StatsSystem
{
    public class InitStats : MonoBehaviour
    {
        [SerializeField] RStats stats;

        private void Awake() => stats.Init();
    }
}
