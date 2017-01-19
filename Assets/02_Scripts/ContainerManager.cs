using UnityEngine;
using System.Collections.Generic;
using SimpleLibrary;

public class ContainerManager : Singleton<ContainerManager>
{
	public List<ContainerInfo> Container = new List<ContainerInfo>();

	public static Container SpawnContainer(Vector3 pos, int containerType)
	{
		return Instance.SpawnContainerByType(pos, containerType);
	}

	public Container SpawnContainerByType(Vector3 pos, int type)
	{
		if (type < 0 || type >= Container.Count)
			return null;
		return Container[type].Spawn(pos);
	}
}