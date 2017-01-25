using System.Collections;
using UnityEngine;

public class Attackable : Moveable
{
    [SerializeField]
    private float baseHealth = 100f;

    public virtual float BaseHealth
    {
        get
        {
            return baseHealth;
        }
    }

    public virtual bool IsAlive
    {
        get
        {
            return Health > 0f;
        }
    }

    public virtual bool IsDead
    {
        get
        {
            return !IsAlive;
        }
    }

    public virtual float MaxHealth
    {
        get
        {
            return BaseHealth;
        }
    }

    public float HealthPercentage
    {
        get
        {
            if (MaxHealth <= 0)
                return 0f;
            return Health / MaxHealth;
        }
    }

    public virtual float RelativeStrength
    {
        get
        {
            return 1f;
        }
    }

    public bool ReleaseSoulWhenDead = true;

    [ReadOnly]
    [SerializeField]
    protected float health;

    public virtual float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    protected override void UpdateAnimator()
    {
        if (Animator != null && Animator.isInitialized)
        {
            Animator.SetFloat("Health", HealthPercentage);
            Animator.SetBool("Alive", IsAlive);
        }
    }

    protected override void Reset()
    {
        Health = MaxHealth;
        base.Reset();
    }

    public virtual float Damage(float amount, Owner attacker)
    {
        if (amount == 0)
            return 0f;
        if (amount < 0)
        {
            Heal(-amount, attacker);
            return 0f;
        }
        float before = Health;
        Health = Mathf.Max(Health - amount, 0f);

        if (Health == 0f)
        {
            StartCoroutine(Die(attacker));
        }
        return before - Health;
    }

    public virtual void Heal(float amount, Owner healer)
    {
        if (amount == 0)
            return;
        if (amount < 0)
        {
            Damage(-amount, healer);
            return;
        }
        Health = Mathf.Min(Health + amount, MaxHealth);
    }

    protected virtual void ReleaseSouls(Owner owner)
    {
        if (owner != null)
            owner.AddSoul(transform.position);
    }

    protected virtual IEnumerator Die(Owner attacker)
    {
        yield return null;
        if (ReleaseSoulWhenDead)
            ReleaseSouls(attacker);
        Despawn();
    }
}