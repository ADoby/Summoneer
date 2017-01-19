using System.Collections;
using UnityEngine;

public class VisibilityEnabler : MonoBehaviour
{
	public RectTransform rectToCheck;
	public GameObject objectToActivate;
	public SimpleLibrary.Timer VisibilityCheckTimer = new SimpleLibrary.Timer(0.05f);

	public float CheckFailureThreshhold = -1f;

	[ReadOnly]
	public float currentPosition;

	[ReadOnly]
	public bool shouldBeActive = false;

	private void Awake()
	{
		if (rectToCheck == null)
			rectToCheck = GetComponent<RectTransform>();
	}

	private void Update()
	{
		if (VisibilityCheckTimer.UpdateAutoReset())
		{
			Check();
		}
	}

	private void Check()
	{
		bool shouldBeActive = false;
		currentPosition = rectToCheck.position.x - Screen.width / 2f;

		shouldBeActive = !(currentPosition - CheckFailureThreshhold < -Screen.width || currentPosition + CheckFailureThreshhold > Screen.width);

		objectToActivate.SetActive(shouldBeActive);
	}
}