using System.Collections;
using UnityEngine;

[System.Serializable]
public class MinionLevelInfo
{
	[Header("Visual Setup")]
	public GameObject TargetObject;

	public Transform UperBoundary;
	public Transform Body;
	public SpriteRenderer[] Sprites;
	public BoxCollider2D collider;
	public Animator Animator;

	[Header("Attributes")]
	public float Health = 100f;

	public float HealthRegeneration = 10f;
	public float HealthRegenCooldown = 2f;
	public float RelativeStrength = 1f;

	public float SightRange = 10f;
	public float AttackRangeX = 1f;
	public float AttackRangeY = 0.2f;

	[Range(0.5f, 20f)]
	public float Acceleration = 2f;

	public float MaxSpeed = 5f;
	public float RandomMovement = 0.2f;

	public float Power = 1f;
	public float ActionStartTime = 0.1f;
	public float ActionCooldown = 0.5f;

	public float Experience = 100f;

	public float WantedDistanceToOtherThingsX = 1f;
	public float WantedDistanceToOtherThingsY = 0.1f;
	public bool AnimationCanMove = true;

	public float BodyCenterYDiff
	{
		get
		{
			return collider.offset.y;
		}
	}

	public void Hide()
	{
		collider.enabled = false;
		TargetObject.SetActive(false);
	}

	public void Show()
	{
		TargetObject.SetActive(true);
		collider.enabled = true;
	}
}