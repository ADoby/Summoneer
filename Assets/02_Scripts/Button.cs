using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[System.Serializable]
	public class CanvasSetting
	{
		public CanvasRenderer Canvas;
		public Color DefaultColor = Color.white;
		public Color HoverColor = Color.gray;
		public Color DownColor = Color.gray;
		public float Speed = 2f;
	}

	public enum States
	{
		DEFAULT,
		HOVER,
		DOWN
	}

	[ReadOnly]
	public States State;

	public CanvasSetting[] Canvases;
	public SimpleLibrary.Timer HoldTimer = new SimpleLibrary.Timer(0.5f);

	private Color CurrentColor;
	private Color WantedColor;

	private void Awake()
	{
		SetState(States.DEFAULT, true);
	}

	private void Update()
	{
		UpdateColor();
		if (State == States.DOWN && !HoldTimer.Finished)
		{
			if (HoldTimer.Update())
				OnHoldClick();
		}
	}

	private void UpdateColor(bool instant = false)
	{
		for (int i = 0; i < Canvases.Length; i++)
		{
			switch (State)
			{
				case States.DEFAULT:
					WantedColor = Canvases[i].DefaultColor;
					break;

				case States.HOVER:
					WantedColor = Canvases[i].HoverColor;
					break;

				case States.DOWN:
					WantedColor = Canvases[i].DownColor;
					break;

				default:
					WantedColor = Canvases[i].DefaultColor;
					break;
			}
			CurrentColor = Canvases[i].Canvas.GetColor();
			if (instant)
				CurrentColor = WantedColor;
			else
				CurrentColor = Color.Lerp(CurrentColor, WantedColor, Time.deltaTime * Canvases[i].Speed);
			Canvases[i].Canvas.SetColor(CurrentColor);
		}
	}

	private void SetState(States state, bool instant = false)
	{
		State = state;

		if (instant)
			UpdateColor(instant);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		OnEnter();
		SetState(States.HOVER);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		OnExit();
		SetState(States.DEFAULT);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		OnDown();
		SetState(States.DOWN);

		HoldTimer.Reset();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		OnUp();
		if (State == States.DOWN)
		{
			OnClick();
			SetState(States.HOVER);
		}
	}

	protected virtual void OnHoldClick()
	{
	}

	protected virtual void OnClick()
	{
	}

	protected virtual void OnEnter()
	{
	}

	protected virtual void OnExit()
	{
	}

	protected virtual void OnDown()
	{
	}

	protected virtual void OnUp()
	{
	}
}