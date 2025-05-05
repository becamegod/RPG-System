using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;
    protected void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }
        Instance = GetComponent<T>();
    }
    //protected void OnDestroy()
    //{
    //    if (Instance == this) Instance = null;
    //}
}

public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            instance ??= new GameObject(typeof(T).Name.SplitPascalCase()).AddComponent<T>();
            return instance;
        }
    }

    private void OnDestroy() => instance = null;
}

