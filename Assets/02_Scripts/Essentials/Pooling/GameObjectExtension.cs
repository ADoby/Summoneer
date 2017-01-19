using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static GameObject Spawn(this GameObject prefab)
    {
        return PoolManager.SpawnOrCreate(prefab);
    }

    /*public static void Despawn(this GameObject instance)
    {
        var info = instance.GetComponent<PooledBehaviour>();
        if (info != null)
            info.Despawn();
    }*/
}