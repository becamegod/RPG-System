using System;
using System.Collections.Generic;
using UnityEngine;

namespace CutsceneSystem
{
    public class AnimatorIdManager : BehaviorIdManager<Animator>
    {
        //private Dictionary<string, Animator> animatorMap;

        //private new void Awake()
        //{
        //    base.Awake();
        //    animatorMap = new();
        //}

        //internal void RegisterId(string id, Animator target)
        //{
        //    if (animatorMap.ContainsKey(id))
        //    {
        //        Debug.LogWarning($"Register error: Animator with id {id} is already existed");
        //        return;
        //    }
        //    animatorMap[id] = target;
        //}

        //internal void DeregisterId(string id)
        //{
        //    if (animatorMap.ContainsKey(id)) animatorMap.Remove(id);
        //    else Debug.LogWarning($"Deregister error: Animator with id {id} is not existed");
        //}

        //internal Animator GetById(string id)
        //{
        //    if (animatorMap.ContainsKey(id)) return animatorMap[id];
        //    Debug.LogWarning($"Query error: Animator with id {id} is not existed");
        //    return null;
        //}
    }
}
