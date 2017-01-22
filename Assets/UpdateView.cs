using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UpdateView : SimpleMVCSBehaviour
{
    public static UnityAction OnUpdate;
    public static UnityAction OnFixedUpdate;

    private void Update()
    {
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate.Invoke();
    }
}