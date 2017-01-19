using SimpleLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
	public enum Types
	{
		NEUTRAL,
		AGGRESSIVE,
		PLAYER
	}

	[SerializeField]
	protected int souls = 10;

	[ReadOnly]
	[SerializeField]
	public List<GameObject> FlyingSouls = new List<GameObject>();

	public virtual int Souls
	{
		get
		{
			return souls;
		}
		set
		{
			souls = value;
		}
	}

	public Types Type;
	public int PlayerIndex = 0;
	public List<Minion> Minions = new List<Minion>();

	public Vector3 CurrentTargetPosition = Vector3.zero;
	public Vector3 MinionCenter = Vector3.zero;
	public Vector2 MinionVelocity = Vector3.zero;

	public float WantedSpeed = 0f;

	public float RelativeStrength = 0f;

	protected virtual void Start()
	{
		GameManager.Instance.RegisterOwner(this);
	}

	public virtual void ReleaseMinion(Minion minion)
	{
		if (Minions.Contains(minion))
		{
			RelativeStrength -= minion.RelativeStrength;
			Minions.Remove(minion);
		}
	}

	public virtual void AddSoul(Vector3 position)
	{
		StartCoroutine(GetSoulAnimation(position));
	}

	public IEnumerator GetSoulAnimation(Vector3 startPosition)
	{
		Transform rect = GameManager.Instance.SoulPrefab.Spawn().transform;
		FlyingSouls.Add(rect.gameObject);
		rect.localScale = Vector3.one;

		float lerp = 0f;
		while (lerp < 1f)
		{
			lerp += Time.deltaTime;
			rect.position = Vector3.Lerp(startPosition, GetSoulStartPosition(), Mathf.Clamp01(lerp));
			yield return null;
		}

		FlyingSouls.Remove(rect.gameObject);
		rect.gameObject.Despawn();
		Souls++;
	}

	public virtual void AddMinion(Minion minion, bool force = false)
	{
		if (minion == null)
			return;
		if (Minions.Contains(minion))
			return;
		Minions.Add(minion);
		minion.Owner = this;

		RelativeStrength += minion.RelativeStrength;
	}

	public void SetMinionTarget()
	{
		CurrentTargetPosition = Utils.ConstraintPositionToLevel(CurrentTargetPosition);
		for (int i = 0; i < Minions.Count; i++)
		{
			if (Minions[i] != null) Minions[i].TargetPosition = CurrentTargetPosition;
		}
	}

	public virtual Vector3 GetSoulStartPosition()
	{
		return transform.position;
	}

	protected virtual void Update()
	{
		SetMinionTarget();
		MinionVelocity = Vector2.zero;
		MinionCenter = Vector3.zero;
		for (int i = 0; i < Minions.Count; i++)
		{
			if (Minions[i] == null)
				continue;
			MinionCenter += Minions[i].transform.position;
			MinionVelocity += Minions[i].Velocity;
		}
		if (Minions.Count > 0)
			MinionCenter /= Minions.Count;
		if (Minions.Count > 0)
			MinionVelocity /= Minions.Count;
	}

	public virtual void IHaveSeenAttackable(Attackable other)
	{
	}

	public virtual void IHaveBeenAttacked(Attackable other)
	{
	}

	protected virtual void Die()
	{
		GameManager.Instance.UnregisterOwner(this);
		for (int i = 0; i < FlyingSouls.Count; i++)
		{
			if (FlyingSouls[i] != null)
			{
				FlyingSouls[i].Despawn();
			}
		}
		FlyingSouls.Clear();
		Destroy(gameObject);
	}

	protected virtual void Awake()
	{
	}

	protected virtual void OnDestroy()
	{
	}

	protected virtual void SpawnMinion(int index)
	{
		if (Souls > 0)
		{
			Minion minion = MinionManager.SpawnMinion(MinionCenter, index);
			AddMinion(minion, true);
			Souls--;
		}
	}
}