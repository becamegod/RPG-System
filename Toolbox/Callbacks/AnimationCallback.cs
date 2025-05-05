using System;

using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    public event Action<string> OnCall;

    public void AddEvent(AnimationClip clip, int normalizedTime, string keyword)
    {
        clip.AddEvent(new AnimationEvent()
        {
            time = clip.length * normalizedTime,
            functionName = nameof(Callback),
            stringParameter = keyword
        });
    }

    public void Callback(string keyword) => OnCall?.Invoke(keyword);
    public void CallbackReference(ScriptableReference keyword) => OnCall?.Invoke(keyword.name);
}
