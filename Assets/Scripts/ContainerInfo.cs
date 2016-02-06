using System.Collections;
using UnityEngine;

[System.Serializable]
public class ContainerInfo
{
	public GameObject Prefab;

	public Container Spawn(Vector3 pos)
	{
		return Prefab.Spawn(pos).GetComponent<Container>();
	}
}