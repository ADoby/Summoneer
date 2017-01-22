using SimpleLibrary;
using System.Collections;
using UnityEngine;

public class KI_Owner : Owner
{
    public MinionSpawnInfos StartInfo = null;

    public Owner TargetOwner = null;
    public Attackable TargetObject = null;
    public float NewRandomTargetDistance = 1f;
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

    private int wantedNextMinion = -1;
    public Timer SpawnMinionCooldown = new Timer(2f);

    public bool HasTarget
    {
        get
        {
            if (TargetOwner != null)
            {
                if (Vector3.Distance(TargetOwner.MinionCenter, MinionCenter) > ForgetDistance)
                    TargetOwner = null;
            }
            if (TargetObject != null)
            {
                if (Vector3.Distance(TargetObject.Position, MinionCenter) > ForgetDistance)
                    TargetObject = null;
            }
            return TargetOwner != null && TargetObject != null;
        }
    }

    public Vector3 TargetPosition
    {
        get
        {
            if (TargetOwner != null)
                CurrentTargetPosition = TargetOwner.MinionCenter;
            if (TargetObject != null)
                CurrentTargetPosition = TargetObject.Position;
            return CurrentTargetPosition;
        }
        set
        {
            CurrentTargetPosition = value;
        }
    }

    public enum States
    {
        IDLE,
        WALKING,
        ATTACKING,
        RUNNING
    }

    public States State = States.IDLE;

    public float TargetStrengthDifference(Owner other)
    {
        if (other == null)
            return 0f;
        return RelativeStrength - other.RelativeStrength;
    }

    protected override void Start()
    {
        base.Start();
        CurrentTargetPosition = transform.position;

        if (StartInfo != null)
        {
            Minion[] minions = StartInfo.Spawn(TargetPosition);
            for (int i = 0; i < minions.Length; i++)
            {
                AddMinion(minions[i], true);
            }
        }
        IdleTimer.Reset();
        UpdateRecruitableMinions.Reset();
        SpawnMinionCooldown.Reset();
    }

    protected virtual void UpdateAttacking()
    {
        if (TargetOwner != null)
        {
            if (TargetOwner.Minions.Count <= 0)
            {
                TargetOwner = null;
            }
        }
        if (TargetObject != null)
        {
            if (TargetObject.IsDead)
            {
                TargetObject = null;
            }
        }
        if (!HasTarget)
        {
            State = States.IDLE;
            IdleTimer.Reset();
            return;
        }
        else
        {
            State = States.ATTACKING;
        }

        //If i am weaker or same strength run away
        if (TargetStrengthDifference(TargetOwner) <= 0)
        {
            State = States.RUNNING;
        }

        Debug.DrawLine(MinionCenter, TargetPosition, Color.red);
    }

    protected virtual void UpdateRunning()
    {
        if (HasTarget)
        {
            TargetPosition = MinionCenter - (TargetOwner.MinionCenter - TargetPosition).normalized * RunDistance;
            Debug.DrawLine(MinionCenter, TargetPosition, Color.blue);

            if (TargetStrengthDifference(TargetOwner) > 0)
                State = States.ATTACKING;

            if (Vector3.Distance(TargetOwner.CurrentTargetPosition, TargetPosition) > SaveDistance)
            {
                TargetOwner = null;
                State = States.IDLE;
            }
        }
        else
        {
            TargetOwner = null;
            State = States.IDLE;
        }
    }

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

    protected override void DoUpdate()
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

        if (HasTarget)
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
            UpdateIdle();
            Debug.DrawLine(MinionCenter, TargetPosition);
        }

        base.DoUpdate();
        transform.position = MinionCenter;

        if (Minions.Count == 0 && FlyingSouls.Count == 0)
        {
            Die();
        }
    }

    public virtual void SetCurrentAttacker(Owner attacker)
    {
        TargetOwner = attacker;
    }

    public override bool DoDamageToTarget(Attackable target, float damage, Attacker attacker)
    {
        bool result = base.DoDamageToTarget(target, damage, attacker);
        if (target == TargetObject && target.IsDead)
        {
            TargetObject = null;
        }
        return result;
    }

    protected virtual void UpdateIdle()
    {
        if (State == States.IDLE)
        {
            //As long as we are idling
            if (!IdleTimer.Update())
                return; //Do nothing

            //Else walk somewhere
            State = States.WALKING;

            TargetPosition = Utils.PositionInLevel();
        }
        else
        {
            //Check if close enough
            if (Vector3.Distance(TargetPosition, MinionCenter) < NewRandomTargetDistance)
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
        if (TargetOwner != null)
            return;
        if (TargetObject != null)
            return;
        if (other != null)
        {
            TargetObject = other;
            State = States.ATTACKING;
        }
    }

    public override void IHaveBeenAttacked(Owner other)
    {
        base.IHaveBeenAttacked(other);
        if (Type == Types.NEUTRAL)
            return;
        if (other != null)
        {
            if (TargetStrengthDifference(other) <= 0)
            {
                //Run away
                TargetOwner = other;
                State = States.RUNNING;
            }
            else
            {
                //Fight
                TargetOwner = other;
                State = States.ATTACKING;
            }
        }
    }
}