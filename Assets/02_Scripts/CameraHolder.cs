using SimpleLibrary;
using System.Collections;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
	public float Speed = 2f;

	public Transform CamTransform;

	[ReadOnly]
	public Camera cam;

	[ReadOnly]
	public new Rigidbody rigidbody;

	[ReadOnly]
	public Vector3 TargetPosition;

	private void Awake()
	{
		cam = CamTransform.GetComponent<Camera>();
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			StartEarthQuake(TestEarthQuakePower);
		}
	}

	private void FixedUpdate()
	{
		TargetPosition.z = transform.position.z;
		rigidbody.AddForce((TargetPosition - transform.position) * Time.fixedDeltaTime * Speed);
		EarthQuake();
	}

	#region CameraShake

	[Header("EarthQuake Setup")]
	public Timer EarthQuakeTimer = new Timer() { Time1 = 0.5f };

	public float MaximumAngel = 45f;
	public float TestEarthQuakePower = 0.5f;

	public float CameraShakeSpeed = 2f;
	private float randomStart = 0f;
	private float ShakePower = 2f;

	public void StartEarthQuake(float power)
	{
		EarthQuakeTimer.Reset();
		ShakePower = power;
		randomStart = Random.Range(-1000f, 1000f);
	}

	public void EarthQuake()
	{
		if (!EarthQuakeTimer.Finished)
		{
			EarthQuakeTimer.Update();

			float damper = 1.0f - Mathf.Clamp(4.0f * EarthQuakeTimer.Procentage - 3.0f, 0.0f, 1.0f);

			float alpha = randomStart + CameraShakeSpeed * EarthQuakeTimer.Procentage;

			float x = Mathf.PerlinNoise(alpha, 0f) * 2f - 1f;
			float y = Mathf.PerlinNoise(0f, alpha) * 2f - 1f;
			float angle = Mathf.PerlinNoise(alpha, alpha) * 2f - 1f;
			x *= ShakePower * damper;
			y *= ShakePower * damper;
			angle *= ShakePower * damper;
			CamTransform.localPosition = new Vector3(x, y, 0f);

			CamTransform.rotation = Quaternion.Euler(0, 0, angle * MaximumAngel);

			if (EarthQuakeTimer.Finished)
			{
				CamTransform.localPosition = Vector3.zero;
				CamTransform.rotation = Quaternion.identity;
			}
		}
	}

	#endregion CameraShake
}