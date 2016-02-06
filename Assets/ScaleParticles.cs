using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleParticles : MonoBehaviour
{
	private new ParticleSystem particleSystem;

	public float BaseSize = 1f;
	public float SizeMultiplier = 1f;
	public float BaseSpeed = 1f;
	public float SpeedMultiplier = 1f;

	public Transform target;

	private void Update()
	{
		if (particleSystem == null)
			particleSystem = GetComponent<ParticleSystem>();
		if (particleSystem != null && target != null)
		{
			particleSystem.transform.localScale = Vector3.one * BaseSize + Vector3.one * (target.lossyScale.magnitude - 1f) * SizeMultiplier;
			particleSystem.startSpeed = BaseSpeed + (transform.lossyScale.magnitude - 1f) * SpeedMultiplier;
		}
	}
}