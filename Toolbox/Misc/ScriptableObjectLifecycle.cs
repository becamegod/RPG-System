using UnityEngine;

public class ScriptableObjectLifecycle : MonoBehaviour
{
    [SerializeField] ScriptableObjectWithLifecycle scriptableObject;

    private void Awake() => scriptableObject.OnStart();
    private void OnDestroy() => scriptableObject.OnEnd();
}

public abstract class ScriptableObjectWithLifecycle : ScriptableObject, ILifecycle
{
    public abstract void OnStart();
    public abstract void OnEnd();
}

public interface ILifecycle
{
    void OnStart();
    void OnEnd();
}
