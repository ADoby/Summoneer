using System.Collections;
using UnityEngine;

public class Attacker : Attackable
{
	protected bool Attacking = false;

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
}