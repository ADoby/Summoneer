using SimpleLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    /*
    public static GameObject Spawn(this GameObject prefab)
    {
        return null;
        //return SimplePool.Spawn(prefab);
    }*/

    /*public static void Despawn(this GameObject instance)
    {
        var info = instance.GetComponent<PooledBehaviour>();
        if (info != null)
            info.Despawn();
    }*/

    public static bool Contains(this Animator _Anim, string _ParamName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _ParamName) return true;
        }
        return false;
    }
}