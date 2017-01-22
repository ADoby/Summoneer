using System.Collections;
using UnityEngine;

public class Attacker : Attackable
{
    [SerializeField]
    [ReadOnly]
    protected bool Attacking = false;

    public virtual bool MoveWhileActioning
    {
        get
        {
            return false;
        }
    }

    [SerializeField]
    private float attackRangeX = 1f;

    public virtual float AttackRangeX
    {
        get
        {
            return attackRangeX;
        }
        set
        {
            attackRangeX = value;
        }
    }

    [SerializeField]
    private float attackRangeY = 1f;

    public virtual float AttackRangeY
    {
        get
        {
            return attackRangeY;
        }
        set
        {
            attackRangeY = value;
        }
    }

    [SerializeField]
    private float power = 5f;

    public virtual float Power
    {
        get
        {
            return power;
        }
    }

    [SerializeField]
    [ReadOnly]
    protected float experience = 5f;

    public virtual float Experience
    {
        get
        {
            return experience;
        }
        set
        {
            experience = value;
        }
    }

    public void AddExperience(float value)
    {
        Experience += value;
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (Animator != null && Animator.isInitialized)
        {
            Animator.SetBool("Attacking", Attacking);
        }
    }

    protected virtual bool CanAttack(Attackable other)
    {
        if (!ShouldAttack(other))
            return false;
        return GameManager.Instance.CanAttack(this, other);
    }

    protected virtual bool ShouldAttack(Attackable other)
    {
        if (other == null)
            return false;
        if (other.IsDead)
            return false;
        if (Owner != null && other.Owner == Owner)
            return false;
        return true;
    }
}