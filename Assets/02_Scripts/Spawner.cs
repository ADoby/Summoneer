using SimpleLibrary;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Spawner
{
	public Timer Timer;

	public float MinDifficulty = 0f;
	public float MaxDifficulty = float.MaxValue;
	public float Chance = 1f;

	public void Start()
	{
		Timer.Reset();
	}

	public void Update(float difficulty)
	{
		if (difficulty < MinDifficulty || difficulty > MaxDifficulty)
			return;
		if (Timer.UpdateAutoReset() && Random.value < Chance)
			Do();
	}

	protected virtual void Do()
	{
	}
}