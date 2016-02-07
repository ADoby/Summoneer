using SimpleLibrary;
using System.Collections;
using UnityEngine;

public class KI_Owner : Owner
{
	public MinionSpawnInfos StartInfo = null;

	public Attackable Target = null;
	public float NewRandomTargetDistance = 2f;
	public Timer UpdateRecruitableMinions = new Timer(2f);
	public Timer IdleTimer = new Timer(2f, 10f);

	public bool CanRecruitMinions = false;

	public float KISightRange = 10f;

	public int MaxRecruitableMinionsChecked = 20;

	[Header("Running")]
	public float RunDistance = 10f;

	public float SaveDistance = 15f;

	[Header("Attacking")]
	public float ForgetDistance = 15f;

	public enum States
	{
		IDLE,
		WALKING,
		ATTACKING,
		RUNNING
	}

	public States State = States.IDLE;

	public float TargetStrengthDifference(Attackable other)
	{
		if (other == null)
			return 0f;
		if (other.Owner == null)
			return RelativeStrength - other.RelativeStrength;
		return RelativeStrength - other.Owner.RelativeStrength;
	}

	protected override void Start()
	{
		base.Start();
		CurrentTargetPosition = transform.position;

		if (StartInfo != null)
		{
			Minion[] minions = StartInfo.Spawn(CurrentTargetPosition);
			for (int i = 0; i < minions.Length; i++)
			{
				RecruitMinion(minions[i], true);
			}
		}
	}

	protected virtual void UpdateAttacking()
	{
		if (Vector3.Distance(CurrentTargetPosition, MinionCenter) > ForgetDistance)
		{
			Target = null;
			State = States.IDLE;
			return;
		}
		CurrentTargetPosition = Target.transform.position;

		if (Target.Health == 0f)
		{
			if (Target.Owner != null)
			{
				if (Target.Owner.Minions.Count > 0)
				{
					Target = Target.Owner.Minions[0];
				}
				else
				{
					Target = null;
				}
			}
			else
			{
				Target = null;
			}
			if (Target == null)
			{
				State = States.IDLE;
				IdleTimer.Reset();
				return;
			}
			else
			{
				State = States.ATTACKING;
			}
		}

		//If i am weaker or same strength run away
		if (TargetStrengthDifference(Target) <= 0)
		{
			State = States.RUNNING;
		}

		Debug.DrawLine(MinionCenter, CurrentTargetPosition, Color.red);
	}

	protected virtual void UpdateRunning()
	{
		if (Target.Owner != null)
		{
			CurrentTargetPosition = MinionCenter - (Target.Owner.MinionCenter - CurrentTargetPosition).normalized * RunDistance;
			Debug.DrawLine(MinionCenter, CurrentTargetPosition, Color.blue);

			if (TargetStrengthDifference(Target) > 0)
				State = States.ATTACKING;

			if (Vector3.Distance(Target.Owner.CurrentTargetPosition, CurrentTargetPosition) > SaveDistance)
			{
				Target = null;
				State = States.IDLE;
			}
		}
		else
		{
			Target = null;
		}
	}

	private int wantedNextMinion = -1;
	public Timer SpawnMinionCooldown = new Timer(2f);

	protected virtual void UpdateRecruiting()
	{
		if (wantedNextMinion < 0)
		{
			//Calculate next wanted minion
			if (SpawnMinionCooldown.Update())
			{
				wantedNextMinion = 0;
			}
		}
		else
		{
			//Wait until enough souls, then buy minion
			if (Souls > 0)
			{
				SpawnMinion(wantedNextMinion);
				SpawnMinionCooldown.Reset();
				wantedNextMinion = -1;
			}
		}
	}

	protected override void Update()
	{
		if (State == States.IDLE)
			WantedSpeed = 0.1f;
		if (State == States.ATTACKING)
			WantedSpeed = 0.7f;
		if (State == States.RUNNING)
			WantedSpeed = 0.9f;
		if (State == States.WALKING)
			WantedSpeed = 0.3f;

		UpdateRecruiting();

		if (Target != null)
		{
			if (State == States.ATTACKING)
			{
				UpdateAttacking();
			}
			else if (State == States.RUNNING)
			{
				UpdateRunning();
			}
		}
		else
		{
			FindRandomTargetPosition();
			Debug.DrawLine(MinionCenter, CurrentTargetPosition);
		}

		base.Update();
		transform.position = MinionCenter;

		if (Type == Types.AGGRESSIVE && CanRecruitMinions)
		{
			if (UpdateRecruitableMinions.UpdateAutoReset())
			{
				TryRecruit(Souls);
			}
		}

		if (Minions.Count == 0 && FlyingSouls.Count == 0)
		{
			Die();
		}
	}

	public virtual void SetCurrentAttacker(Attacker attacker)
	{
		Target = attacker;
	}

	protected virtual void FindRandomTargetPosition()
	{
		if (State == States.IDLE)
		{
			//As long as we are idling
			if (!IdleTimer.Update())
				return; //Do nothing

			//Else walk somewhere
			State = States.WALKING;

			if (Souls > 0 && RecruitableMinion.Count > 0)
			{
				//Try to get to recruitable Minions
				for (int i = 0; i < Mathf.Min(RecruitableMinion.Count, MaxRecruitableMinionsChecked); i++)
				{
					if (GameManager.Instance.InRecruitRange(this, RecruitableMinion[i]))
					{
						//In range, walk tawards
						CurrentTargetPosition = RecruitableMinion[i].transform.position;
						return;
					}
				}
			}
			CurrentTargetPosition = Utils.PositionInLevel();
		}
		else
		{
			//Check if close enough
			if (Vector3.Distance(CurrentTargetPosition, MinionCenter) < NewRandomTargetDistance)
			{
				State = States.IDLE;
				IdleTimer.Reset();
			}
		}
	}

	public override void IHaveSeenAttackable(Attackable other)
	{
		base.IHaveSeenAttackable(other);
		if (Type == Types.NEUTRAL)
			return;
		IHaveBeenAttacked(other);
	}

	public override void IHaveBeenAttacked(Attackable other)
	{
		base.IHaveBeenAttacked(other);
		if (Type == Types.NEUTRAL)
			return;
		if (other.Owner != null)
		{
			if (TargetStrengthDifference(other) <= 0)
			{
				//Run away
				Target = other;
				State = States.RUNNING;
			}
			else
			{
				//Fight
				Target = other;
				State = States.ATTACKING;
			}
		}
		else
		{
			//Destroy if we are not attacking something yet
			if (Target == null)
			{
				Target = other;
				State = States.ATTACKING;
			}
		}
	}

	public override void CanRecruit(Minion minion)
	{
		base.CanRecruit(minion);
		if (Type == Types.AGGRESSIVE && CanRecruitMinions)
		{
			TryRecruit(minion);
		}
	}
}