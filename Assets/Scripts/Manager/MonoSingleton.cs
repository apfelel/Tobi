using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogWarning(typeof(T).Name + "Missing");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = (T)this;
    }
}
