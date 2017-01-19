using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SkillButton : Button
{
	public enum Types
	{
		NOTHING,
		SUMMONMINION
	}
	public Types Type;
	//Properties
	public int Index = 0;

	//Events
	[System.Serializable]
	public class SkillButtonEvent : UnityEvent<SkillButton> { }
	public SkillButtonEvent ClickEvent = new SkillButtonEvent();
	public SkillButtonEvent HoldClickEvent = new SkillButtonEvent();

	public static SkillButtonEvent SkillButtonClicked = new SkillButtonEvent();
	public static SkillButtonEvent SkillButtonHold = new SkillButtonEvent();

	protected override void OnHoldClick()
	{
		HoldClickEvent.Invoke(this);
		SkillButtonHold.Invoke(this);
	}
	protected override void OnClick()
	{
		ClickEvent.Invoke(this);
		SkillButtonClicked.Invoke(this);
	}
}