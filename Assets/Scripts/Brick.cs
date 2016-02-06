using SimpleLibrary;
using System.Collections;
using UnityEngine;

public class Brick : MonoBehaviour
{
	public SpriteRenderer sprite;
	public Rigidbody2D rigid;
	public float UpForce = 5f;
	public float RightLeftForce = 5f;

	private bool lost = false;
	public Timer LifeTime = new Timer(3f);
	public AnimationCurve FadeCurve = new AnimationCurve() { keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) } };

	private Color color;

	public void Despawn()
	{
		if (Application.isPlaying)
			gameObject.Despawn();
		else
			DestroyImmediate(gameObject);
	}

	public void SetColor(Color color)
	{
		this.color = color;
		sprite.color = color;
	}

	private void Update()
	{
		if (lost)
		{
			LifeTime.Update();
			color.a = FadeCurve.Evaluate(LifeTime.Procentage);
			sprite.color = color;
			if (LifeTime.Finished)
				Despawn();
		}
	}

	public void Losen()
	{
		lost = true;
		LifeTime.Reset();
		rigid.isKinematic = false;
		rigid.AddForce(Vector3.up * UpForce + Vector3.right * Random.Range(-1, 1) * RightLeftForce, ForceMode2D.Impulse);
	}
}