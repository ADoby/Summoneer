using UnityEngine;
using System.Collections;
using System;

public class GameEvents
{
	public static event Action<Minion> MinionRecruited;
	public static void TriggerMinionRecruited(Minion minion)
	{
		if (MinionRecruited != null)
			MinionRecruited.Invoke(minion);
	}
	public static event Action<Minion> MinionReleased;
	public static void TriggerMinionReleased(Minion minion)
	{
		if (MinionReleased != null)
			MinionReleased.Invoke(minion);
	}
}