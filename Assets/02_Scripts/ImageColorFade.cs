using System.Collections;
using UnityEngine;

public class ImageColorFade : MonoBehaviour
{
	public SimpleLibrary.Timer TransitionTimer = new SimpleLibrary.Timer() { Time1 = 0.5f };

	/// <summary>
	/// Curve default = smooth in/out
	/// </summary>
	public AnimationCurve Curve = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) } };

	public bool StartAs = false;

	public bool IsActive = false;
	public bool InstantStart = true;

	public UnityEngine.UI.Image BG;
	public UnityEngine.UI.RawImage BG2;
	public CanvasRenderer canvas;
	public CanvasGroup group;
	public Material FadeMaterial;
	public string FadeMaterialColorName = "_Color";
	public Color ActiveBG = Color.white;
	public Color DeactiveBG = new Color(1, 1, 1, 0);

	public bool DeactivateWhenFadeDeactive = true;
	private float lastValue = -1f;

	public delegate void NothingDelegate();

	private NothingDelegate callback = null;

	public float Value
	{
		get
		{
			return IsActive ? 1f - TransitionTimer.Procentage : TransitionTimer.Procentage;
		}
	}

	private void Awake()
	{
		Reset();
	}

	public void Reset()
	{
		BG2 = GetComponent<UnityEngine.UI.RawImage>();

		lastValue = -1f;
		callback = null;
		bool wanted2 = IsActive;
		bool wanted = StartAs;
		IsActive = !wanted;
		Set(wanted, true);

		IsActive = !wanted2;
		Set(wanted2, InstantStart);
	}

	public void SetSmooth(bool value)
	{
		Set(value, false);
	}

	public void SetInvertedSmooth(bool value)
	{
		Set(!value, false);
	}

	public void SetInstant(bool value)
	{
		Set(value, true);
	}

	public void SetInvertedInstant(bool value)
	{
		Set(!value, true);
	}

	public void Set(bool value, bool instant = false, NothingDelegate callback2 = null)
	{
		if (value == IsActive)
			return;
		if (value == true && DeactivateWhenFadeDeactive)
		{
			gameObject.SetActive(true);
		}
		IsActive = value;

		if (TransitionTimer.Procentage > 0.05f)
			TransitionTimer.Reset();
		else
			TransitionTimer.Finish();

		if (instant)
		{
			TransitionTimer.Finish();
			UpdateColor(Value);
		}
		callback = callback2;

		if (group)
			group.blocksRaycasts = IsActive;
	}

	private void Update()
	{
		TransitionTimer.Update();
		UpdateColor(Value);
	}

	private void UpdateColor(float value)
	{
		if (lastValue == value)
			return;

		Color color = Color.Lerp(ActiveBG, DeactiveBG, Curve.Evaluate(value));
		if (BG) BG.color = color;
		if (BG2) BG2.color = color;
		if (canvas) canvas.SetColor(color);
		if (group) group.alpha = color.a;
		if (FadeMaterial) FadeMaterial.SetColor(FadeMaterialColorName, color);

		if (IsActive == false && TransitionTimer.Procentage == 1f)
		{
			if (DeactivateWhenFadeDeactive) gameObject.SetActive(false);
			if (callback != null) callback();
		}
		else if (IsActive && TransitionTimer.Procentage == 1f)
		{
			if (callback != null) callback();
		}
		lastValue = value;
	}

	public void HideDefault()
	{
		Hide();
	}

	public void Hide(bool instant = false, NothingDelegate callback2 = null)
	{
		Set(false, instant, callback2);
	}

	public void ShowDefault()
	{
		Show();
	}

	public void Show(bool instant = false, NothingDelegate callback2 = null)
	{
		Set(true, instant, callback2);
	}

	public void Toggle()
	{
		Set(!IsActive);
	}
}