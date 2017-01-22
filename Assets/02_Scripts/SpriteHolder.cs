using SimpleLibrary;
using System.Collections;
using UnityEngine;

public class SpriteHolder : PooledBehaviour
{
    public SpriteRenderer[] sprites;
    public bool EveryFrame = true;

    private MaterialPropertyBlock block;

    public virtual SpriteRenderer[] Sprites
    {
        get
        {
            return sprites;
        }
    }

    public Transform Bottom;

    protected override void Awake()
    {
        UpdateView.OnUpdate += DoUpdate;
        Do();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        UpdateView.OnUpdate -= DoUpdate;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Do();
    }

    protected virtual void DoUpdate()
    {
        if (!EveryFrame)
            return;
        Do();
    }

    private void Do()
    {
        for (int i = 0; i < Sprites.Length; i++)
        {
            if (Sprites[i] != null)
                Sprites[i].sortingOrder = (int)(Bottom.position.y * -50f) + i;
        }
        DoShader();
    }

    public void DoShader()
    {
        if (Sprites == null || Sprites == null)
            return;
        if (block == null)
            block = new MaterialPropertyBlock();
        for (int i = 0; i < Sprites.Length; i++)
        {
            if (Sprites[i] == null)
                continue;
            if (block != null) block.Clear();

            Sprites[i].GetPropertyBlock(block);
            block.SetVector("_Tint", Sprites[i].color);
            block.SetVector("_ObjectPosition", transform.position);
            Sprites[i].SetPropertyBlock(block);
        }
    }
}