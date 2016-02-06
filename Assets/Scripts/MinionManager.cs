using SimpleLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : Singleton<MinionManager>
{
	public List<MinionInfo> Minions = new List<MinionInfo>();

	public static Minion SpawnMinion(Vector3 pos, int minionType)
	{
		return Instance.SpawnMinionByType(pos, minionType);
	}

	public Minion SpawnMinionByType(Vector3 pos, int type)
	{
		if (type < 0 || type >= Minions.Count)
			return null;
		return Minions[type].Spawn(pos);
	}
}