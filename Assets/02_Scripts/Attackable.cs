using System.Collections;
using UnityEngine;

public class Attackable : SpriteHolder
{
    [ReadOnly]
    public Owner Owner;

    [SerializeField]
    private Animator animator;

    public virtual Animator Animator
    {
        get
        {
            return animator;
        }
    }

    [SerializeField]
    [ReadOnly]
    private new Rigidbody2D rigidbody;

    public virtual Rigidbody2D Rigidbody

    {
        get
        {
            return rigidbody;
        }
    }

    public Vector2 Position
    {
        get
        {
            if (Rigidbody)
                return Rigidbody.position;
            return transform.position;
        }
        set
        {
            if (Rigidbody)
                Rigidbody.position = value;
            transform.position = value;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            if (Rigidbody)
                return Rigidbody.velocity;
            return Vector2.zero;
        }
        set
        {
            if (Rigidbody)
                Rigidbody.velocity = value;
        }
    }

    public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
    {
        if (Rigidbody != null)
            Rigidbody.AddForce(force, mode);
    }

    [SerializeField]
    private float baseHealth = 100f;

    public virtual float BaseHealth
    {
        get
        {
            return baseHealth;
        }
    }

    [SerializeField]
    private new BoxCollider2D collider;

    public virtual BoxCollider2D Collider

    {
        get
        {
            return collider;
        }
    }

    [SerializeField]
    private Transform body;

    public virtual Transform Body
    {
        get
        {
            return body;
        }
    }

    [SerializeField]
    private float bodyCenterYDiff;

    public virtual float BodyCenterYDiff
    {
        get
        {
            return bodyCenterYDiff;
        }
    }

    public virtual Vector3 BodyCenter
    {
        get
        {
            if (Collider != null)
            {
                return body.TransformPoint(Collider.offset);
            }
            return body.position;
        }
    }

    public virtual float SizeX
    {
        get
        {
            if (Collider == null)
                return 0f;
            return Collider.size.x;
        }
    }

    public virtual float SizeY
    {
        get
        {
            if (Collider == null)
                return 0f;
            return Collider.size.y;
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

    protected override void Awake()
    {
        base.Awake();
        Init();
        Reset();
    }

    private bool initialized = false;

    protected virtual void Init()
    {
        if (initialized)
            return;
        initialized = true;
        if (animator == null) animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void DoUpdate()
    {
        base.DoUpdate();
        UpdateAnimator();
    }

    protected virtual void UpdateAnimator()
    {
        if (Animator != null && Animator.isInitialized)
        {
            Animator.SetFloat("Health", HealthPercentage);
            Animator.SetBool("Alive", IsAlive);
        }
    }

    protected virtual void Reset()
    {
        Health = MaxHealth;
        UpdateAnimator();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Reset();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
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