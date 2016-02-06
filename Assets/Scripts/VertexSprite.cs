using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VertexSprite : MonoBehaviour
{
	public SpriteHolder Sprites;
	void Update()
	{
		if (enabled && !Application.isPlaying) Sprites.DoShader();
	}
}