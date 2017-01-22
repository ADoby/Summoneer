using System.Collections;
using UnityEngine;

[System.Serializable]
public class MinionInfo
{
    public GameObject Prefab;

    public Minion Spawn(Vector3 pos)
    {
        return (Minion)Prefab.Spawn(pos);
    }
}