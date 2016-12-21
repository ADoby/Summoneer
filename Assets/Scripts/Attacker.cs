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

    public virtual bool DoDamageToTarget(Attackable target)
    {
        if (CanAttack(target))
        {
            float damageDone = target.Damage(Power, this);
            Experience += GameManager.Instance.CalculateDamageDoneExp(damageDone, RelativeStrength, target.RelativeStrength);
            if (target.IsDead)
            {
                Experience += GameManager.Instance.CalculateEnemyKilledExp(RelativeStrength, target.RelativeStrength);
            }
            return true;
        }
        return false;
    }

    protected virtual bool CanAttack(Attackable other)
    {
        if (!ShouldAttack(other))
            return false;
        return GameManager.Instance.CanAttack(this, other);
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (Animator != null)
        {
            Animator.SetBool("Attacking", Attacking);
        }
    }

    protected virtual bool ShouldAttack(Attackable other)
    {
        if (other == null)
            return false;
        if (other.IsDead)
            return false;
        if (other.Owner != null && other.Owner == Owner)
            return false;
        return true;
    }
}