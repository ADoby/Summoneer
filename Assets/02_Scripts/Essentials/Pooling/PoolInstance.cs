using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolInstance

{
    public List<PooledBehaviour> Queue;

    public List<PooledBehaviour> AktiveInstances;

    private int counter;
    private static PooledBehaviour tmpObject;

    public PoolInstance(PoolInfo info)
    {
        counter = 0;
        AktiveInstances = new List<PooledBehaviour>();
        Queue = new List<PooledBehaviour>();
        AddInstances(info);
    }

    private PooledBehaviour Dequeue()
    {
        if (Queue.Count == 0)
            return null;
        tmpObject = Queue[0];

        Queue.RemoveAt(0);
        return tmpObject;
    }

    private void Enqueue(PooledBehaviour script)
    {
        if (!Queue.Contains(script))
            Queue.Add(script);
    }

    public void AddInstances(PoolInfo info)
    {
        for (int i = 0; i < info.InitialCount; i++)
        {
            AddInstance(info);
        }
    }

    public void AddInstance(PoolInfo info)
    {
        tmpObject = CreateInstance(info);
        AddEntity(tmpObject);
    }

    private PooledBehaviour CreateInstance(PoolInfo info)
    {
        var go = Object.Instantiate(info.Prefab);
        go.name = string.Format("{0}_{1}", info.Prefab.name, counter);
        go.transform.SetParent(info.Parent);
        counter++;
        var script = go.GetComponent<PooledBehaviour>();
        if (script == null)
            script = go.AddComponent<PooledBehaviour>();
        script.OnDespawn();
        return script;
    }

    public PooledBehaviour GetEntity(PoolInfo info)
    {
        PooledBehaviour script = null;
        if (Queue.Count > 0)
            script = Dequeue();

        if (script == null)
            script = CreateInstance(info);

        AktiveInstances.Add(script);
        script.OnSpawn();
        return script;
    }

    public void AddEntity(PooledBehaviour script)
    {
        if (AktiveInstances.Contains(script))
            AktiveInstances.Remove(script);
        if (!Queue.Contains(script))
            Enqueue(script);
    }

    public void Despawn(PooledBehaviour instance)
    {
        instance.OnDespawn();
        AddEntity(instance);
    }
}