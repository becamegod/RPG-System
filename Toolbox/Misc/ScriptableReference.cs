using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Reference")]
public class ScriptableReference : ScriptableObject
{
    public static implicit operator string(ScriptableReference reference) => reference.name;
}
