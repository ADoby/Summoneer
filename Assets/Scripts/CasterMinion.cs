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
		Projectile projectile = Projectile.Spawn(BodyCenter).GetComponent<Projectile>();
		projectile.SetSize(ProjectileSize);
		projectile.SetTarget(target, this);
		projectile.Damage = Power;
	}
}