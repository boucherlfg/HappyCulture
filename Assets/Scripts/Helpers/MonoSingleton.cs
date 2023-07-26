using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance
    {
        get;
        private set;
    }
    protected virtual void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            Debug.LogWarning($"two instances of mono singleton {typeof(T).Name} detected. only one shall remain.");
        }
        else
        {
            Instance = this as T;
        }
    }
}