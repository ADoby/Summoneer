using System.Collections;
using UnityEngine;

[System.Serializable]
public class ContainerDifficultySetting
{
	public int ContainerID = 0;
	public float MinDifficulty = 0f;
	public float MaxDifficulty = float.MaxValue;

	[Range(0f, 1f)]
	public float Chance = 1f;
}