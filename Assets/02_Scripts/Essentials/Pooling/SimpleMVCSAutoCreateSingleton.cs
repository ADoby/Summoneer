using System.Collections.Generic;
using UnityEngine;

public class SimpleMVCSAutoCreateSingleton<T> : SimpleMVCSSingleton<T> where T : SimpleMVCSBehaviour
{
    public new static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();
            if (instance == null)
            {
                var go = new GameObject(string.Format("{0}_Instance", typeof(T)));
                instance = go.AddComponent<T>();
            }
            return instance;
        }
    }
}