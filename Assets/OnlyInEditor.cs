using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyInEditor : MonoBehaviour
{
    public bool DestroyOnPlay = true;

    private void Awake()
    {
        if (DestroyOnPlay)
            Destroy(gameObject);
    }
}