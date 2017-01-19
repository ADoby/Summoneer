using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using SimpleLibrary;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class Building : Attacker
{
	public Transform BrickParent;
	public GameObject Brick;
	public List<Brick> Bricks = new List<Brick>();
	public SortedList<float, List<Brick>> BricksToLose = new SortedList<float, List<Brick>>();
	public List<SpriteRenderer> BrickSprites = new List<SpriteRenderer>();

	public float BrickWidth = 1f;
	public float BrickHeight = 1f;

	private Brick brick;

	public new BoxCollider2D Collider;
	public Transform Shadow;

	[Header("Settings")]
	public float LightChance = 0.4f;

	public float LineChance = 0.4f;

	public Color WallColor = new Color(0.4f, 0.4f, 0.4f);
	public Color DarkWallColor = new Color(0.3f, 0.3f, 0.3f);
	public Color LightColor = new Color(0.6f, 1.0f, 0.6f);
	public Color Light2Color = new Color(1f, 0.6f, 0.6f);
	public Color LightOffColor = new Color(0.2f, 0.2f, 0.2f);
	public Color AntenaColor = new Color(0.2f, 0.2f, 0.2f);
	public Color AntenaBoxColor = new Color(0.2f, 0.2f, 0.2f);

	private Brick SpawnBrick(Vector3 localPos, Color color)
	{
		if (Application.isPlaying)
			brick = Brick.Spawn(false).GetComponent<Brick>();
		else
		{
#if UNITY_EDITOR
			brick = (PrefabUtility.InstantiatePrefab(Brick) as GameObject).GetComponent<Brick>();
#endif
		}
		brick.transform.SetParent(BrickParent);
		brick.transform.localPosition = localPos;
		brick.SetColor(color);

		Bricks.Add(brick);
		AddBrickToLose(brick);
		BrickSprites.Add(brick.sprite);

		return brick;
	}

	private void AddBrickToLose(Brick brick)
	{
		float y = 200 - brick.transform.localPosition.y;
		if (!BricksToLose.ContainsKey(y))
		{
			BricksToLose.Add(y, new List<global::Brick>());
		}
		BricksToLose[y].Add(brick);
	}

	protected override void Awake()
	{
		base.Awake();
		for (int i = 0; i < Bricks.Count; i++)
		{
			AddBrickToLose(Bricks[i]);
		}
	}

	private void DespawnOld()
	{
		for (int i = 0; i < Bricks.Count; i++)
		{
			if (Bricks[i] != null)
				Bricks[i].Despawn();
		}
		Bricks.Clear();
		BrickSprites.Clear();
		BricksToLose.Clear();
	}

	public void GenerateBuilding()
	{
		DespawnOld();

		GenerateType1();
	}

	[SerializeField]
	[ReadOnly]
	private float damaged = 0f;

	[SerializeField]
	[ReadOnly]
	private float DamagePerBrick = 1f;

	private float BrickWeight(Brick brick)
	{
		return 1f;
	}

	public override float Damage(float amount, Attacker attacker)
	{
		float damageDone = base.Damage(amount, attacker);
		damaged += damageDone;

		if (BricksToLose.Count > 0)
		{
			int count = (int)(damaged / DamagePerBrick);
			for (int i = 0; i < count; i++)
			{
				if (BricksToLose.Count == 0)
					break;
				if (BricksToLose[BricksToLose.Keys[0]].Count == 0)
				{
					BricksToLose.RemoveAt(0);
					i--;
					continue;
				}
				brick = BricksToLose[BricksToLose.Keys[0]].RandomEntry(BrickWeight);
				if (brick == null)
				{
					i--;
					continue;
				}
				brick.Losen();
				BricksToLose[BricksToLose.Keys[0]].Remove(brick);
			}

			damaged -= count * DamagePerBrick;
		}

		return damageDone;
	}

	protected override void Despawn()
	{
		StartCoroutine(DespawnLater(3f));
	}

	private IEnumerator DespawnLater(float time)
	{
		yield return new WaitForSeconds(time);

		gameObject.Despawn();
	}

	private void GenerateType1()
	{
		int width = Random.Range(6, 10);
		int height;
		if (width < 8)
			height = Random.Range(12, 18);
		else
			height = Random.Range(8, 14);

		bool line = false;
		bool light = false;

		Vector3 pos = Vector3.zero;
		Color color = Color.gray;
		for (int y = 0; y < height; y++)
		{
			light = Random.Range(0f, 1f) > LightChance;
			line = Random.Range(0f, 1f) > LineChance;
			for (int x = 0; x < width; x++)
			{
				if (y > 0 && y < height - 2 && y % 2 == 0)
				{
					if (x >= 1 && x < 3)
					{
						if (light)
							color = LightColor;
						else
							color = LightOffColor;
					}
					else if (line && x >= 3)
						color = DarkWallColor;
					else
						color = WallColor;
					//Fenster
				}
				else
				{
					color = WallColor;
				}
				pos.x = x * BrickWidth;
				pos.y = y * BrickHeight;
				SpawnBrick(pos, color);
			}
		}

		AddTop(height, width);

		Collider.size = new Vector2(width * 0.25f, 0.25f);
		Collider.offset = new Vector2(width * 0.125f, 0.125f);

		Shadow.localScale = new Vector3(width * 0.5f, 4f, 1f);
		Shadow.localPosition = new Vector3(Collider.offset.x, -0.1f, 0);

		sprites = BrickSprites.ToArray();

		DamagePerBrick = MaxHealth / Bricks.Count;
	}

	private void AddTop(float y, float width)
	{
		int type = Random.Range(0, 3);

		Vector3 pos = Vector3.zero;
		Color color = Color.gray;
		int count = 0;
		float lx;

		for (int x = 0; x < width; x++)
		{
			if (x >= 1 && x < width - 1)
				color = DarkWallColor;
			else
				color = WallColor;

			pos.x = x * BrickWidth;
			pos.y = y * BrickHeight;
			SpawnBrick(pos, color);

			if (type == 0)
			{
				if (x == 2)
				{
					count = Random.Range(4, 6);
					color = AntenaColor;
					for (int i = 0; i < count; i++)
					{
						pos.y += BrickHeight;
						SpawnBrick(pos, color);
					}
				}
				if (x == 4 && width > 6)
				{
					count = Random.Range(1, 3);
					color = AntenaColor;
					for (int i = 0; i < count; i++)
					{
						pos.y += BrickHeight;
						SpawnBrick(pos, color);
					}
				}
			}
			else if (type == 1 && width > 5)
			{
				if (x == width - 3)
				{
					count = Random.Range(3, 5);
					for (int i = 0; i < count; i++)
					{
						pos.y += BrickHeight;
						if (i == count - 1)
							color = Random.value > 0.5f ? LightColor : Light2Color;
						SpawnBrick(pos, color);
					}
				}
			}
			else if (type == 2 && width > 5)
			{
				if (x == width - 3)
				{
					count = Random.Range(3, 4);
					for (int i = 0; i < count; i++)
					{
						pos.y += BrickHeight;
						if (i >= count - 2)
						{
							color = AntenaBoxColor;
							lx = pos.x;
							for (int a = -1; a <= 1; a++)
							{
								pos.x = lx + a * BrickWidth;
								SpawnBrick(pos, color);
							}
							pos.x = lx;
						}
						else
						{
							color = AntenaColor;
							SpawnBrick(pos, color);
						}
					}
				}
			}
		}
	}
}