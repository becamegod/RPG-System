using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace StatsSystem
{
    static class Global
    {
        public const string AssetMenu = "Stats System/";
    }

    public abstract class BaseStat
    {
        //public abstract float Value { get; set; }

        [SerializeField] protected float value;
        public float Value
        {
            get => value;
            set
            {
                if (this.value == value) return;
                var delta = value - this.value;
                this.value = value;

                OnChanged?.Invoke(delta);
            }
        }

        public static implicit operator float(BaseStat stat) => stat.Value;
        public event Action<float> OnChanged;
    }

    public abstract class BaseStat<T> : BaseStat, IName, ICloneable
    {
        [SerializeField] protected T name;
        //[SerializeField] float value;

        public abstract string Name { get; }
        //public override float Value
        //{
        //    get => value;
        //    set => this.value = value;
        //}

        public static implicit operator float(BaseStat<T> stat) => stat.Value;

        public BaseStat(BaseStat<T> source)
        {
            value = source.value;
            name = source.name;
        }

        public abstract object Clone();

        #region operator overload
        //public static BaseStat<T> operator +(BaseStat<T> a, float b)
        //{
        //    a.value += b;
        //    return a;
        //}
        //public static BaseStat<T> operator -(BaseStat<T> a, float b)
        //{
        //    a.value -= b;
        //    return a;
        //}
        //public static BaseStat<T> operator *(BaseStat<T> a, float b)
        //{
        //    a.value *= b;
        //    return a;
        //}
        //public static BaseStat<T> operator /(BaseStat<T> a, float b)
        //{
        //    a.value /= b;
        //    return a;
        //}
        #endregion
    }

    public abstract class BaseStats<T> : ScriptableObject where T : IName, ICloneable
    {
        [SerializeField] protected List<T> stats;
        List<T> currentStats;

        List<T> CurrentStats
        {
            get
            {
                if (currentStats == null || currentStats.Count == 0) Init();
                return currentStats;
            }
            set => currentStats = value;
        }

        T Find(List<T> list, string name) => list.Find(stat => stat.Name == name);
        public T this[string name] => Find(CurrentStats, name);
        public T GetBase(string name) => Find(stats, name);

        public void Init() => CurrentStats = stats.Clone().ToList();
    }

    public class StatsInfo : Info
    {
        [SerializeField] RStats stats;
        public RStats Stats => stats;
    }
}