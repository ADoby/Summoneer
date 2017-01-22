using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolInfo
{
    public string Name = "Pool";
    public GameObject Prefab;
    public Transform Parent;
    public bool CreateParent = false;
    public int InitialCount = 0;

    public PoolInstance Pool;

    public PoolInfo(string name)
    {
        Name = name;
    }

    public PoolInfo(PoolInfo copyFrom, Transform parent, GameObject prefab)
    {
        Prefab = prefab;
        InitialCount = copyFrom.InitialCount;
        Parent = copyFrom.Parent;
        Init(copyFrom, parent);
    }

    public void Init(PoolInfo copyFrom, Transform parent)
    {
        if (string.IsNullOrEmpty(Name))
            CreateName(copyFrom);
        if (copyFrom.CreateParent)
        {
            Parent = new GameObject(Name).transform;
            Parent.SetParent(parent);
        }
    }

    public void CreateName(PoolInfo copyFrom)
    {
        Name = copyFrom.Name;
        if (Prefab == null)
            return;
        if (Name.Contains("{0}"))
            Name = string.Format(Name, Prefab.name);
    }

    public bool IsInfo(GameObject prefab)
    {
        return Prefab == prefab;
    }

    public GameObject CreateInstance()
    {
        return Pool.GetEntity(this).gameObject;
    }

    public void Despawn(PooledBehaviour instance)
    {
        Pool.Despawn(instance);
    }
}