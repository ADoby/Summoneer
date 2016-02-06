using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyDifficultySetting
{
	public int MinionID = 0;
	public float MinDifficulty = 0f;
	public float MaxDifficulty = float.MaxValue;
	public int MinCount = 0;
	public int MaxCount = 1;
}