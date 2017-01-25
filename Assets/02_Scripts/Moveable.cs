using System.Collections;
using UnityEngine;

public class Moveable : SpriteHolder
{
    #region Inspector

    [SerializeField]
    private Animator animator;

    [SerializeField]
    [ReadOnly]
    private Vector2 velocity;

    [SerializeField]
    private float damping;

    [SerializeField]
    private float maxSpeed;

    [SerializeField]
    private new BoxCollider2D collider;

    [SerializeField]
    private Transform body;

    [SerializeField]
    private float bodyCenterYDiff;

    #endregion Inspector

    #region Properties

    [ReadOnly]
    public Owner Owner;

    public virtual Animator Animator
    {
        get
        {
            return animator;
        }
    }

    private Transform _transform;

    public new Transform transform
    {
        get
        {
            if (_transform == null)
                _transform = transform;
            return _transform;
        }
    }

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    public virtual BoxCollider2D Collider

    {
        get
        {
            return collider;
        }
    }

    public virtual Transform Body
    {
        get
        {
            return body;
        }
    }

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

    public virtual float Damping
    {
        get
        {
            return damping;
        }
    }

    public virtual float Speed
    {
        get
        {
            return Velocity.magnitude;
        }
    }

    public virtual float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
    }

    public virtual bool HasAnimator
    {
        get
        {
            return animator != null && animator.isInitialized;
        }
    }

    #endregion Properties

    protected bool initialized = false;

    protected override void Awake()
    {
        base.Awake();
        Init();
        Reset();
    }

    protected virtual void Init()
    {
        if (initialized)
            return;
        initialized = true;
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        UpdateVelocity();
        UpdateMovement();
        UpdateAnimator();
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Force)
    {
        if (forceMode == ForceMode2D.Impulse)
            Velocity += force;
        else
            Velocity += force * Time.deltaTime;
        if (Owner != null)
            Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed * Owner.WantedSpeed);
        else
            Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);
    }

    protected virtual void UpdateVelocity()
    {
        Velocity -= Vector2.ClampMagnitude(Velocity.normalized * Damping * Time.deltaTime, Velocity.magnitude);
    }

    protected virtual void UpdateMovement()
    {
        transform.position += (Vector3)Velocity * Time.deltaTime;
    }

    protected virtual void UpdateAnimator()
    {
        if (HasAnimator && Animator.Contains("Speed"))
        {
            Animator.SetFloat("Speed", Speed);
        }
    }

    protected virtual void Reset()
    {
        Velocity = Vector3.zero;
        UpdateAnimator();
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Reset();
    }
}