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
	public List<Minion> RecruitableMinion = new List<Minion>();

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

	public virtual void RecruitMinion(Minion minion, bool force = false)
	{
		if (minion == null)
			return;
		if (Minions.Contains(minion))
			return;
		minion.Recruit(this, force);
		Minions.Add(minion);

		RelativeStrength += minion.RelativeStrength;
		CanNoLongerRecruit(minion);
	}

	public void SetMinionTarget()
	{
		CurrentTargetPosition = Utils.ConstraintPositionToLevel(CurrentTargetPosition);
		for (int i = 0; i < Minions.Count; i++)
		{
			if (Minions[i] != null) Minions[i].TargetPosition = CurrentTargetPosition;
		}
	}

	public virtual void CanRecruit(Minion minion)
	{
		if (GameManager.Instance.InRecruitRange(this, minion))
		{
			if (!RecruitableMinion.Contains(minion))
				RecruitableMinion.Add(minion);
		}
	}

	public virtual void CanNoLongerRecruit(Minion minion)
	{
		if (RecruitableMinion.Contains(minion))
			RecruitableMinion.Remove(minion);
	}

	public virtual void TryRecruit(int amount)
	{
		if (Souls < amount)
			amount = Souls;

		for (int i = 0; i < RecruitableMinion.Count; i++)
		{
			if (TryRecruit(RecruitableMinion[i]))
				amount--;
			if (amount == 0)
				break;
		}
	}

	protected virtual bool TryRecruit(Minion minion)
	{
		if (Souls <= 0)
			return false;
		if (GameManager.Instance.InRecruitRange(this, minion))
		{
			StartCoroutine(RecruitMinionLater(minion));
			return true;
		}
		return false;
	}

	public virtual Vector3 GetSoulStartPosition()
	{
		return transform.position;
	}

	public IEnumerator RecruitMinionLater(Minion minion)
	{
		if (Souls <= 0)
		{
			yield break;
		}
		Souls--;
		Transform rect = GameManager.Instance.SoulPrefab.Spawn().transform;
		FlyingSouls.Add(rect.gameObject);

		rect.localScale = Vector3.one;

		float lerp = 0f;
		while (lerp < 1f)
		{
			//if it gets claimed while we fly
			if (minion.Owner != null)
				break;
			lerp += Time.deltaTime;
			rect.position = Vector3.Lerp(GetSoulStartPosition(), minion.Body.position, Mathf.Clamp01(lerp));
			yield return null;
		}

		//Only really recruit if minion is free anymore
		if (minion.Owner == null)
		{
			RecruitMinion(minion, false);
		}
		else
		{
			lerp = 0f;
			Vector3 start = rect.position;
			while (lerp < 1f)
			{
				lerp += Time.deltaTime;
				rect.position = Vector3.Lerp(start, GetSoulStartPosition(), Mathf.Clamp01(lerp));
				yield return null;
			}
			Souls++;
		}
		FlyingSouls.Remove(rect.gameObject);
		rect.gameObject.Despawn();
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
	{ }

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
		GameEvents.MinionRecruited += OnMinionRecruited;
		GameEvents.MinionReleased += OnMinionReleased;
	}

	protected virtual void OnDestroy()
	{
		GameEvents.MinionRecruited -= OnMinionRecruited;
		GameEvents.MinionReleased -= OnMinionReleased;
	}

	protected virtual void OnMinionReleased(Minion minion)
	{
		CanRecruit(minion);
	}

	protected virtual void OnMinionRecruited(Minion minion)
	{
		CanNoLongerRecruit(minion);
	}
}