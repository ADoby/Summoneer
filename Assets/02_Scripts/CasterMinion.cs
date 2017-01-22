using System.Collections.Generic;
using UnityEngine;

public class CasterMinion : Minion
{
    [Header("Caster")]
    public List<CasterLevelInfo> CasterLevelInfos = new List<CasterLevelInfo>();

    public CasterLevelInfo CurrentCasterLevelInfo
    {
        get
        {
            return CasterLevelInfos[Level];
        }
    }

    public GameObject Projectile
    {
        get
        {
            return CurrentCasterLevelInfo.Projectile;
        }
    }

    public float ProjectileSize
    {
        get
        {
            return CurrentCasterLevelInfo.ProjectileSize;
        }
    }

    protected override void DoAction()
    {
        Attackable target = null;
        for (int i = 0; i < EnemiesNear.Count; i++)
        {
            if (CanAttack(EnemiesNear[i]))
            {
                target = EnemiesNear[i];
                break;
            }
        }
        Shoot(target);
    }

    protected virtual void Shoot(Attackable target)
    {
        if (target == null)
            return;
        if (Projectile == null)
        {
            Debug.LogWarning(string.Format("No projectile setup for {0} at level {1}", gameObject.name, Level), gameObject);
            return;
        }
        var script = Projectile.Spawn(BodyCenter);
        if (script == null)
        {
            Debug.LogError("Spawned Projectile is null", gameObject);
            return;
        }
        Projectile projectile = (Projectile)script;
        projectile.SetSize(ProjectileSize);
        projectile.SetTarget(target, this, Owner);
        projectile.Damage = Power;
    }
}