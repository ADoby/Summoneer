using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Owner
{
	public CameraHolder cam;
	public GameObject CanRecruitUIPrefab;
	public GameObject SoulPrefab;
	public Vector3 SoulStartPosition;
	public float SoulTravelTime = 1f;
	public AnimationCurve SoulTravelAnimation = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) } };
	public MinionSpawnInfos StartInfo;

	public override int Souls
	{
		get
		{
			return souls;
		}
		set
		{
			souls = value;
			SoulCountText.text = string.Format("{0:0}", souls);
		}
	}

	public Text SoulCountText;

	public Dictionary<Minion, FollowUI> SpawnedRecruitNotifier = new Dictionary<Minion, FollowUI>();

	protected override void Start()
	{
		CurrentTargetPosition = Vector3.zero;

		Minion[] minions = StartInfo.Spawn(transform.position);
		for (int i = 0; i < minions.Length; i++)
		{
			RecruitMinion(minions[i], true);
		}
		Souls = souls;
		GameManager.Instance.RegisterPlayer(this);
	}

	protected override void Update()
	{
		if (Input.GetButton("Move"))
		{
			CurrentTargetPosition = cam.cam.ScreenToWorldPoint(Input.mousePosition);
			CurrentTargetPosition.z = 0;
		}
		base.Update();
		cam.TargetPosition = MinionCenter;

		if (Input.GetButtonDown("Recruit"))
			TryRecruit(Souls);
	}

	public override void CanRecruit(Minion minion)
	{
		base.CanRecruit(minion);
		if (SpawnedRecruitNotifier.ContainsKey(minion))
		{
			return;
		}
		if (!GameManager.Instance.InRecruitRange(this, minion))
			return;
		FollowUI follow = CanRecruitUIPrefab.Spawn().GetComponent<FollowUI>();
		SpawnedRecruitNotifier.Add(minion, follow);
		follow.Set(minion.UperBoundary);
	}

	public override void CanNoLongerRecruit(Minion minion)
	{
		base.CanNoLongerRecruit(minion);
		if (SpawnedRecruitNotifier.ContainsKey(minion))
			SpawnedRecruitNotifier[minion].Set(null);
	}

	public override Vector3 GetSoulStartPosition()
	{
		return cam.cam.ViewportToWorldPoint(SoulStartPosition);
	}

	public void CastSkill(SkillButton button)
	{
		if (button.Type == SkillButton.Types.SUMMONMINION)
		{
			SpawnMinion(button.Index);
		}
	}
}