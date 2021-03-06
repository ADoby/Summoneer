﻿using SimpleLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Attacker
{
    private const float MinSpeed = 0f;
    private const float SpeedMult = 5000f;

    [Header("Setup")]
    public List<MinionLevelInfo> LevelInfos = new List<MinionLevelInfo>();

    #region LEVELINFO

    public MinionLevelInfo CurrentLevelInfo
    {
        get
        {
            return LevelInfos[Level];
        }
    }

    public MinionLevelInfo LastLevelInfo
    {
        get
        {
            if (Level == 0)
                return null;
            return LevelInfos[Level - 1];
        }
    }

    public override float BaseHealth
    {
        get
        {
            return CurrentLevelInfo.Health;
        }
    }

    public MinionLevelInfo NextLevelInfo
    {
        get
        {
            if (Level >= LevelInfos.Count - 1)
                return CurrentLevelInfo;
            return LevelInfos[Level + 1];
        }
    }

    public override Animator Animator
    {
        get
        {
            return CurrentLevelInfo.Animator;
        }
    }

    public override SpriteRenderer[] Sprites
    {
        get
        {
            return CurrentLevelInfo.Sprites;
        }
    }

    public SpriteRenderer[] NextLevelSprites
    {
        get
        {
            return NextLevelInfo.Sprites;
        }
    }

    public override float MaxHealth
    {
        get
        {
            return CurrentLevelInfo.Health;
        }
    }

    public float HealthRegen
    {
        get
        {
            return CurrentLevelInfo.HealthRegeneration;
        }
    }

    public override float RelativeStrength
    {
        get
        {
            return CurrentLevelInfo.RelativeStrength;
        }
    }

    public override float Speed
    {
        get
        {
            return CurrentLevelInfo.Acceleration * SpeedMult;
        }
    }

    public override float MaxSpeed
    {
        get
        {
            return CurrentLevelInfo.MaxSpeed;
        }
    }

    public float Randomness
    {
        get
        {
            return CurrentLevelInfo.RandomMovement;
        }
    }

    public override float Power
    {
        get
        {
            return CurrentLevelInfo.Power;
        }
    }

    public override bool MoveWhileActioning
    {
        get
        {
            return CurrentLevelInfo.MoveWhileActioning;
        }
    }

    public Transform UperBoundary
    {
        get
        {
            return CurrentLevelInfo.UperBoundary;
        }
    }

    public override Transform Body
    {
        get
        {
            return CurrentLevelInfo.Body;
        }
    }

    public float NeededExperience
    {
        get
        {
            return CurrentLevelInfo.Experience;
        }
    }

    public override BoxCollider2D Collider
    {
        get
        {
            return CurrentLevelInfo.collider;
        }
    }

    public float SightRange
    {
        get
        {
            return CurrentLevelInfo.SightRange;
        }
        set
        {
            CurrentLevelInfo.SightRange = value;
        }
    }

    public override float AttackRangeX
    {
        get
        {
            return CurrentLevelInfo.AttackRangeX;
        }
        set
        {
            CurrentLevelInfo.AttackRangeX = value;
        }
    }

    public override float AttackRangeY
    {
        get
        {
            return CurrentLevelInfo.AttackRangeY;
        }
        set
        {
            CurrentLevelInfo.AttackRangeY = value;
        }
    }

    public float WantedDistanceToOtherThingsX
    {
        get
        {
            return CurrentLevelInfo.WantedDistanceToOtherThingsX;
        }
        set
        {
            CurrentLevelInfo.WantedDistanceToOtherThingsX = value;
        }
    }

    public float WantedDistanceToOtherThingsY
    {
        get
        {
            return CurrentLevelInfo.WantedDistanceToOtherThingsY;
        }
        set
        {
            CurrentLevelInfo.WantedDistanceToOtherThingsY = value;
        }
    }

    public override float BodyCenterYDiff
    {
        get
        {
            return CurrentLevelInfo.BodyCenterYDiff;
        }
    }

    #endregion LEVELINFO

    public int Level = 0;

    public bool DoingLevelUp = false;
    public bool AnimationCanMove = true;

    public Timer HealthRegenWaitTimer = new Timer() { Time1 = 2f };

    public GameObject ExperienceBarPrefab;
    public ProgressBarUI ExperienceBar;

    public float MaxSpeedDistance = 5f;
    public float DistanceToStopAction = 1f;

    public Timer CheckNearFieldTimer = new Timer() { Time1 = 0.5f };
    public LayerMask SightMask;

    public Timer AttackCooldownTimer = new Timer() { Time1 = 0.5f };

    [Header("Debug Info")]
    [ReadOnly]
    public Vector3 TargetPosition;

    [ReadOnly]
    public float ProcentageSpeed;

    [ReadOnly]
    public float CurrentDistance;

    [ReadOnly]
    public List<Attackable> EnemiesNear = new List<Attackable>();

    [ReadOnly]
    public Collider2D[] ColliderNear = new Collider2D[0];

    //Temp Variables

    private RaycastHit2D SomethingOnTheLeft;
    private RaycastHit2D SomethingOnTheRight;
    private Color color;
    private Coroutine dodge;
    private Vector3 walkDirection;
    private Vector3 randomize = Vector3.zero;
    private AttackableCollider otherCollider;
    private Attackable other;
    private Vector3 diff;

    #region Attackable Properties

    public override Vector3 BodyCenter
    {
        get
        {
            return Body.position + Vector3.up * BodyCenterYDiff;
        }
    }

    #endregion Attackable Properties

    #region Attacker Properties

    public override float Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
            if (ExperienceBar) ExperienceBar.Set(ExperienceProcentage);
            if (experience > NeededExperience)
            {
                StartLevelUp();
            }
        }
    }

    #endregion Attacker Properties

    #region Properties

    public float ExperienceProcentage
    {
        get
        {
            if (NeededExperience <= 0)
                return 0f;
            return Experience / NeededExperience;
        }
    }

    public bool CanMove
    {
        get
        {
            return IsAlive && (!Attacking || !MoveWhileActioning);
        }
    }

    #endregion Properties

    #region Health

    protected virtual void UpdateHealthRegen()
    {
        if (HealthRegenWaitTimer.Update())
            Heal(HealthRegen * Time.deltaTime, Owner);
    }

    public override float Damage(float amount, Owner attacker)
    {
        if (IsDead || attacker == null)
            return 0f;
        float done = base.Damage(amount, attacker);

        if (Health == 0)
            Release(attacker);
        else
        {
            Experience += GameManager.Instance.CalculateDamageSurvivedExp(done, RelativeStrength, attacker.RelativeStrength);
            if (Owner != null)
                Owner.IHaveBeenAttacked(attacker);
        }

        HealthRegenWaitTimer.Time1 = CurrentLevelInfo.HealthRegenCooldown;
        HealthRegenWaitTimer.Reset();
        return done;
    }

    public virtual void Release(Owner byOwner)
    {
        if (Owner != null)
        {
            //Remove Minion from Owner
            Owner.ReleaseMinion(this);
        }
        Owner = null;

        if (byOwner != null)
        {
            //Give Killer Soul
            byOwner.AddSoul(Body.position);
        }

        Collider.enabled = false;
        Velocity = Vector2.zero;
    }

    public void Reanimate()
    {
        Collider.enabled = true;
    }

    //Overwrite Die (default despawns object)
    protected override IEnumerator Die(Owner attacker)
    {
        yield return null;
        if (ReleaseSoulWhenDead)
            ReleaseSouls(attacker);
    }

    public void DoDespawn()
    {
        Despawn();
    }

    #endregion Health

    #region LevelUp

    [ContextMenu("Level Up")]
    public void LevelUp()
    {
        StartLevelUp();
    }

    protected virtual void StartLevelUp()
    {
        //Check if we are at last level (next == current)
        if (CurrentLevelInfo == NextLevelInfo)
        {
            return;
        }

        DoingLevelUp = true;

        //Trigger LevelUp for current level
        //Next Level will get "Spawn"
        if (Animator != null) Animator.SetTrigger("LevelUp");

        SetLevel(Level + 1);
    }

    public virtual void DoLevelUp()
    {
        DoingLevelUp = false;
        if (LastLevelInfo != null)
            LastLevelInfo.Hide();

        //Update Experience
        Experience = Experience;
    }

    private void SetLevel(int level)
    {
        if (level < 0 || level >= LevelInfos.Count)
            return;

        Level = level;
        CurrentLevelInfo.Show();
        AnimationCanMove = CurrentLevelInfo.AnimationCanMove;

        if (Animator != null) Animator.SetTrigger("Spawn");
    }

    public void ShowCurrentLevel()
    {
        for (int i = 0; i < LevelInfos.Count; i++)
        {
            if (i == Level)
                LevelInfos[i].Show();
            else
                LevelInfos[i].Hide();
        }
    }

    protected virtual void ResetLevel()
    {
        for (int i = 0; i < LevelInfos.Count; i++)
        {
            LevelInfos[i].Hide();
        }

        SetLevel(0);
        ShowCurrentLevel();
        Experience = 0f;
        if (ExperienceBar) ExperienceBar.Set(ExperienceProcentage, true);
        Health = BaseHealth;
    }

    public void DoStartMovement()
    {
        AnimationCanMove = true;
    }

    public void DoStopMovement()
    {
        AnimationCanMove = false;
    }

    #endregion LevelUp

    #region Initilization

    protected override void Reset()
    {
        base.Reset();
        ResetAction();
        DoingLevelUp = false;
        TargetPosition = transform.position;
        ResetLevel();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Init();

        if (ExperienceBarPrefab != null)
        {
            var script = ExperienceBarPrefab.Spawn();
            if (script != null)
            {
                ExperienceBar = script.GetComponent<ProgressBarUI>();
                ExperienceBar.follow.Set(UperBoundary);
            }
        }

        Reset();

        dodge = StartCoroutine(Dodge());
    }

    public override void OnDespawn()
    {
        if (ExperienceBar)
        {
            ExperienceBar.Despawn();
        }

        if (dodge != null) StopCoroutine(dodge);
        base.OnDespawn();
    }

    #endregion Initilization

    #region Movement

    protected virtual IEnumerator Dodge()
    {
        int currentIndex = 0;
        while (true)
        {
            if (CanMove)
            {
                if (currentIndex >= ColliderNear.Length)
                    currentIndex = 0;

                if (ColliderNear.Length > 0 && ColliderNear[currentIndex] != null && ColliderNear[currentIndex].enabled)
                    AddForce(CalculateEvation(ColliderNear[currentIndex].transform) * Time.fixedDeltaTime * Speed);
                currentIndex++;
            }

            yield return null;
        }
    }

    protected virtual Vector3 CalculateEvation(Transform other)
    {
        diff = transform.position - other.position;
        diff.x *= 1f - Mathf.Clamp01(Mathf.Abs(diff.x) / WantedDistanceToOtherThingsX);
        diff.y *= 1f - Mathf.Clamp01(Mathf.Abs(diff.y) / WantedDistanceToOtherThingsY);
        return diff;
    }

    #endregion Movement

    #region Actions

    public override void DoUpdate()
    {
        base.DoUpdate();

        if (IsDead)
            return;
        if (DoingLevelUp)
            return;

        UpdateThingsNearMe();
        UpdateHealthRegen();

        if (Attacking)
        {
            //Check if attack can end
            //We only attack, if our master does not want us to move
            if (CurrentDistance < DistanceToStopAction)
            {
                for (int i = 0; i < EnemiesNear.Count; i++)
                {
                    if (CanAttack(EnemiesNear[i]))
                        return;
                }
            }

            ResetAction();
        }
        UpdateAction();
    }

    protected override void UpdateVelocity()
    {
        if (IsDead)
            return;
        if (Attacking && !MoveWhileActioning)
            return;

        if (AnimationCanMove)
        {
            base.UpdateVelocity();

            walkDirection = (TargetPosition - transform.position);
            CurrentDistance = walkDirection.magnitude;
            walkDirection.Normalize();

            ProcentageSpeed = Mathf.Clamp01(CurrentDistance / MaxSpeedDistance);
            if (Owner != null)
                ProcentageSpeed *= Owner.WantedSpeed;

            walkDirection = walkDirection * ProcentageSpeed;

            walkDirection.x += ((Random.value * 2) - 1) * Randomness;
            walkDirection.y += ((Random.value * 2) - 1) * Randomness;
            AddForce(walkDirection * Time.deltaTime * Speed);
        }
    }

    protected virtual void UpdateThingsNearMe()
    {
        if (CheckNearFieldTimer.UpdateAutoReset())
        {
            ColliderNear = Physics2D.OverlapCircleAll(transform.position, SightRange, SightMask);
            EnemiesNear.Clear();

            for (int i = 0; i < ColliderNear.Length; i++)
            {
                otherCollider = ColliderNear[i].GetComponent<AttackableCollider>();
                if (otherCollider == null)
                {
                    ColliderNear[i] = null;
                    continue;
                }
                other = otherCollider.Target;
                if (other == this)
                {
                    ColliderNear[i] = null;
                    continue;
                }
                //If Collider has MinionScript, compare Owners to find out if its an enemy

                if (ShouldAttack(other) && Owner != null)
                    Owner.IHaveSeenAttackable(other);
                if (CanAttack(other))
                    EnemiesNear.Add(other);
            }
        }
    }

    protected virtual void UpdateAction()
    {
        if (AttackCooldownTimer.Update())
        {
            if (!MoveWhileActioning && CurrentDistance > DistanceToStopAction)
            {
                return;
            }

            for (int i = 0; i < EnemiesNear.Count; i++)
            {
                if (CanAttack(EnemiesNear[i]))
                {
                    StartAction();
                    break;
                }
            }
        }
    }

    protected virtual void StartAction()
    {
        Attacking = true;
    }

    public virtual void TriggerDoAction()
    {
        DoAction();
        ResetAction();
    }

    protected virtual void DoAction()
    {
        DoDamageToTargets(EnemiesNear);
    }

    protected virtual void ResetAction()
    {
        Attacking = false;
        AttackCooldownTimer.Time1 = CurrentLevelInfo.ActionCooldown;
        AttackCooldownTimer.Reset();
    }

    protected virtual void DoDamageToTargets(List<Attackable> targets)
    {
        //Default Attack only Attacks one target
        for (int i = 0; i < EnemiesNear.Count; i++)
        {
            if (Owner.DoDamageToTarget(EnemiesNear[i], Power, this))
            {
                return;
            }
        }
    }

    #endregion Actions

    #region Animations

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();

        if (Animator != null && Animator.isInitialized)
        {
            Animator.SetFloat("Speed", ProcentageSpeed);
        }
    }

    #endregion Animations
}