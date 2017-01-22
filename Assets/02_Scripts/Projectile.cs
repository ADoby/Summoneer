using System.Collections.Generic;
using UnityEngine;

public class Projectile : PooledBehaviour
{
    public float Damage = 0f;

    public float Speed = 10f;
    public float StartForce = 1f;
    public float RotateSpeed = 5f;

    public float ExplodeDistance = 0.5f;

    public float ExplosionForce = 5f;
    public float ExplosionRange = 5f;

    public bool RotateTowards = true;

    [ReadOnly]
    public bool isAlive = true;

    public Animator Animator;

    [ReadOnly]
    public Attackable Target;

    [ReadOnly]
    public Attacker Sender;

    [ReadOnly]
    public Owner Owner;

    protected Rigidbody2D rigid;

    private List<Attackable> attackables = new List<Attackable>();
    private Attackable attackable;
    private AttackableCollider attackableCollider;

    protected virtual Vector3 BodyCenter
    {
        get
        {
            return transform.position;
        }
    }

    protected virtual Vector3 Direction
    {
        get
        {
            if (Target == null)
                return Vector3.zero;
            return Target.BodyCenter - BodyCenter;
        }
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    public void SetTarget(Attackable target, Attacker attacker, Owner owner)
    {
        Owner = owner;
        Target = target;
        Sender = attacker;
        rigid.AddForce(Direction.normalized * StartForce, ForceMode2D.Impulse);
        float rot_z = Mathf.Atan2(Direction.normalized.y, Direction.normalized.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        isAlive = true;
        UpdateView.OnUpdate += DoFixedUpdate;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        UpdateView.OnFixedUpdate -= DoFixedUpdate;
    }

    protected override void Awake()
    {
        base.Awake();

        if (Animator == null) Animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    protected virtual void DoFixedUpdate()
    {
        if (!isAlive)
            return;
        if (Target != null)
        {
            rigid.AddForce(transform.up * Speed);
            if (RotateTowards)
            {
                float rot_z = Mathf.Atan2(Direction.normalized.y, Direction.normalized.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * RotateSpeed);
            }
        }
        else
        {
            Explode();
        }

        if (Direction.sqrMagnitude < 0.1f)
            Explode();
    }

    protected virtual void Explode()
    {
        if (!isAlive)
            return;
        rigid.velocity = Vector2.zero;
        isAlive = false;

        if (Owner == null)
        {
            Despawn();
            return;
        }

        if (ExplosionRange > 0)
        {
            attackables.Clear();
            Collider2D[] targets = Physics2D.OverlapCircleAll(rigid.position, ExplosionRange);

            for (int i = 0; i < targets.Length; i++)
            {
                attackable = targets[i].GetComponent<Attackable>();
                attackableCollider = targets[i].GetComponent<AttackableCollider>();
                if (attackable == null && attackableCollider != null)
                    attackable = attackableCollider.Target;

                if (attackable == null)
                    continue;

                if (Owner != null && attackable.Owner == Owner)
                    continue;

                attackables.Add(attackable);
            }
            for (int i = 0; i < attackables.Count; i++)
            {
                ExplodeTarget(attackables[i]);
            }
        }
        else
        {
            ExplodeTarget(Target);
        }
        if (Animator != null)
            Animator.SetTrigger("Explode");
        else
            Despawn();
    }

    protected virtual void ExplodeTarget(Attackable target)
    {
        if (target == null)
            return;

        Owner.DoDamageToTarget(target, Damage, Sender);

        Vector2 force = (target.Position - rigid.position);
        force *= (ExplosionRange - force.magnitude); //Reverse force magnitude (closer = bigger force)
        force *= ExplosionForce;
        if (force.magnitude < 0) //don't suck things in
            force *= 0;

        target.AddForce(force, ForceMode2D.Impulse);
    }
}