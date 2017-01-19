using System.Collections.Generic;
using UnityEngine;

public class SimpleMVCSSingleton<T> : SimpleMVCSBehaviour where T : SimpleMVCSBehaviour
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();
            return instance;
        }
    }

    protected override void Awake()
    {
        if (instance == this)
        {
            Destroy(this);
        }
        else
        {
            instance = this as T;
        }
    }
}