using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpdateView : SimpleMVCSBehaviour
{
    //public static UnityAction OnUpdate;
    public static UnityAction OnFixedUpdate;

    public static List<SpriteHolder> Updater;

    public static void AddUpdater(SpriteHolder updater)
    {
        if (Updater == null)
            Updater = new List<SpriteHolder>();
        if (!Updater.Contains(updater))
            Updater.Add(updater);
    }

    public static void RemoveUpdater(SpriteHolder updater)
    {
        if (Updater == null)
            Updater = new List<SpriteHolder>();
        if (Updater.Contains(updater))
            Updater.Remove(updater);
    }

    protected override void Awake()
    {
        base.Awake();

        if (Updater == null)
            Updater = new List<SpriteHolder>();
    }

    private void Update()
    {
        //if (OnUpdate != null)
        //    OnUpdate.Invoke();
        for (int i = 0; i < Updater.Count; i++)
        {
            if (Updater[i] != null)
                Updater[i].DoUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate.Invoke();
    }
}