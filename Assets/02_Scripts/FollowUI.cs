using System.Collections;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
	[SerializeField]
	[ReadOnly]
	private Transform target;

	private RectTransform rect;
	public float Speed = 5f;
	public ImageColorFade Fader;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
	}

	public void Set(Transform trans)
	{
		target = trans;

		if (target != null)
		{
			rect.anchoredPosition = Camera.main.WorldToScreenPoint(target.position);
			Fader.Show();
		}
		else
		{
			Fader.Hide(false, Despawn);
		}
	}

	private void Despawn()
	{
		gameObject.Despawn();
	}

	private void Update()
	{
		if (target == null)
			return;
		if (Speed <= 0f)
			rect.anchoredPosition = Camera.main.WorldToScreenPoint(target.position);
		else
			rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, Camera.main.WorldToScreenPoint(target.position), Time.deltaTime * Speed);
	}
}