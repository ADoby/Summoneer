using System.Collections;
using UnityEngine;

public class MinionTrigger : MonoBehaviour
{
	public Minion Target;

	public void DoAttack()
	{
		Target.TriggerDoAction();
	}

	public void DoLevelUp()
	{
		Target.DoLevelUp();
	}

	public void DoDespawn()
	{
		Target.DoDespawn();
	}

	public void DoStartMovement()
	{
		Target.DoStartMovement();
	}

	public void DoStopMovement()
	{
		Target.DoStopMovement();
	}
}