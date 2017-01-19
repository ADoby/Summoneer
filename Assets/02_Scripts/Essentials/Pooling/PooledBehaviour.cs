using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledBehaviour : SimpleMVCSBehaviour
{
    [ReadOnly]
    public GameObject Prefab;

    private GameObject owner;

    public GameObject Owner
    {
        get
        {
            if (owner == null)
                owner = gameObject;
            return owner;
        }
    }

    public void Despawn()
    {
        PoolManager.Despawn(this);
    }

    public virtual void OnSpawn()
    {
    }

    public virtual void OnDespawn()
    {
    }
}