using UnityEngine;
using System.Collections;

[System.Serializable]
public class ExperienceInfo
{
	public float DamageDoneToExperience = 0.1f;
	public float DamageSurvivedToExperience = 0.1f;
	public float EnemeyKilledBaseExperience = 50f;
	public float EnemyKilledExperience = 0.5f;
	public float EnemyStrengthExperienceMultiplier = 0.5f;
}