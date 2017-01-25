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
        Do();
    }

    protected virtual void OnEnable()
    {
        UpdateView.AddUpdater(this);
    }

    protected virtual void OnDisable()
    {
        UpdateView.RemoveUpdater(this);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Do();
    }

    public virtual void DoUpdate()
    {
        if (!EveryFrame)
            return;
        Do();
    }

    private void Do()
    {
        DoShader();
    }

    public void DoShader()
    {
        if (Sprites == null || Sprites.Length == 0)
            return;
        if (block == null)
            block = new MaterialPropertyBlock();
        for (int i = 0; i < Sprites.Length; i++)
        {
            if (Sprites[i] == null)
                continue;
            Sprites[i].sortingOrder = (int)(Bottom.position.y * -50f) + i;
            if (block != null) block.Clear();

            Sprites[i].GetPropertyBlock(block);
            block.SetVector("_Tint", Sprites[i].color);
            block.SetVector("_ObjectPosition", transform.position);
            Sprites[i].SetPropertyBlock(block);
        }
    }
}