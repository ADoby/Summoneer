using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class VertexSprite : MonoBehaviour
{
    public SpriteHolder Sprites;

    private void Awake()
    {
        if (Application.isPlaying)
            enabled = false;
    }

    private void Update()
    {
        if (enabled && !Application.isPlaying) Sprites.DoShader();
    }
}