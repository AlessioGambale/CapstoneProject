using System;
using UnityEngine;

public abstract class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance { get => _instance; private set => _instance = value; }

    protected virtual bool ShouldBeDestroyedOnLoad { get; set; } = true;
    public static event Action OnSingletonReady;

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = GetComponent<T>();
        if (!ShouldBeDestroyedOnLoad) DontDestroyOnLoad(gameObject);
        OnSingletonReady?.Invoke();
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
