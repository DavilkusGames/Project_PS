using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool AlwaysKeepAlive = false;
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("An instance already exists");
            Destroy(gameObject);
            return;
        }
        Instance = this as T;
        if (AlwaysKeepAlive) DontDestroyOnLoad(gameObject);
    }
}