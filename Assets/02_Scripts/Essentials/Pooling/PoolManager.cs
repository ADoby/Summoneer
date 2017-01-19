using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SimpleMVCSAutoCreateSingleton<PoolManager>
{
    public override void BindToContext(SimpleContext context)
    {
        base.BindToContext(context);
        Bind(this);
    }

    public override void OnRegister()
    {
        base.OnRegister();

        PoolInfoCache = new Dictionary<GameObject, PoolInfo>();
        //Generate Initial Pools
        for (int i = 0; i < PoolInfos.Count; i++)
        {
            PoolInfos[i].Init(DefaultPoolInfo, transform);
            PoolInfos[i].Pool = new PoolInstance(PoolInfos[i]);
            AddPoolInfo(PoolInfos[i]);
        }
    }

    public PoolInfo DefaultPoolInfo = new PoolInfo("{0}_Pool") { CreateParent = true };

    public List<PoolInfo> PoolInfos;

    public Dictionary<GameObject, PoolInfo> PoolInfoCache;

    public bool HasPool(GameObject prefab)
    {
        return PoolInfoCache.ContainsKey(prefab);
    }

    public bool HasPool(PoolInfo info)
    {
        return PoolInfoCache.ContainsValue(info);
    }

    public PoolInfo GetPoolInfo(GameObject prefab)
    {
        if (PoolInfoCache.ContainsKey(prefab))
            return PoolInfoCache[prefab];
        return DefaultPoolInfo;
    }

    public void CreatePoolInfo(GameObject prefab)
    {
        if (HasPool(prefab))
        {
            return;
        }
        AddPoolInfo(new PoolInfo(DefaultPoolInfo, transform, prefab));
    }

    private void AddPoolInfo(PoolInfo info)
    {
        if (!PoolInfos.Contains(info))
        {
            PoolInfos.Add(info);
        }

        if (HasPool(info.Prefab))
        {
            return;
        }
        if (HasPool(info))
        {
            return;
        }

        PoolInfoCache.Add(info.Prefab, info);
    }

    public static GameObject SpawnOrCreate(GameObject prefab)
    {
        return Instance.SpawnOrCreateObject(prefab);
    }

    public static void Despawn(PooledBehaviour instance)
    {
        Instance.DespawnObject(instance);
    }

    public void DespawnObject(PooledBehaviour instance)
    {
        if (!HasPool(instance.Prefab))
            return;
        PoolInfoCache[instance.Prefab].Despawn(instance);
    }

    public GameObject SpawnOrCreateObject(GameObject prefab)
    {
        if (!HasPool(prefab))
            return null;
        return GetPoolInfo(prefab).CreateInstance();
    }
}