using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MinionSpawnInfo
{
	public int MinionType = 0;
	public int Count = 0;

	public MinionSpawnInfo(int minionType, int count)
	{
		MinionType = minionType;
		Count = count;
	}

	public Minion[] Spawn(Vector3 position)
	{
		Minion[] minions = new Minion[Count];
		for (int i = 0; i < Count; i++)
		{
			minions[i] = MinionManager.SpawnMinion(position, MinionType);
		}
		return minions;
	}
}

[System.Serializable]
public class MinionSpawnInfos
{
	public List<MinionSpawnInfo> Infos = new List<MinionSpawnInfo>();

	public Minion[] Spawn(Vector3 position)
	{
		List<Minion> minions = new List<Minion>();
		for (int i = 0; i < Infos.Count; i++)
		{
			minions.AddRange(Infos[i].Spawn(position));
		}
		return minions.ToArray();
	}
}