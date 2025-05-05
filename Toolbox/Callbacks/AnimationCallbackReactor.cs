using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class AnimationCallbackReactor : MonoBehaviour
{
    [SerializeField] AnimationCallback callback;
    [SerializeField] bool logMissingReaction;

    [Serializable]
    struct Reaction
    {
        enum KeywordType { String, Reference }
        [SerializeField] KeywordType keywordType;
        [SerializeField, ShowWhen(nameof(keywordType), KeywordType.String)] string keyword;
        [SerializeField, ShowWhen(nameof(keywordType), KeywordType.Reference)] ScriptableReference reference;
        [SerializeField] UnityEvent action;

        public string Keyword => keywordType switch
        {
            KeywordType.String => keyword,
            KeywordType.Reference => reference.name,
            _ => ""
        };
        public UnityEvent Action => action;
    }
    [SerializeField] Reaction[] reactions;

    private void Reset() => callback ??= GetComponentInChildren<AnimationCallback>();

    private void OnEnable() => callback.OnCall += OnAnimationEvent;

    private void OnDisable() => callback.OnCall -= OnAnimationEvent;

    private void OnAnimationEvent(string keyword)
    {
        Reaction entry;
        try { entry = reactions.First(entry => entry.Keyword == keyword); }
        catch (InvalidOperationException)
        {
            if (logMissingReaction) Debug.LogWarning($"No reaction found for keyword: {keyword}");
            return;
        }
        entry.Action.Invoke();
    }
}
