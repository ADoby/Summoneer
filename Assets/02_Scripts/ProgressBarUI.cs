using System.Collections;
using UnityEngine;

public class ProgressBarUI : PooledBehaviour
{
    public float MaxSize = 100f;
    public RectTransform ProgressBarRect;
    public float Speed = 2f;

    public FollowUI follow;

    public float StartProcentage = 1f;

    private float procentage = 0f;
    private Vector2 size;

    [Inject]
    public UpdateSignal UpdateSignal { get; set; }

    public void Set(float nvalue, bool instant = false)
    {
        procentage = Mathf.Clamp01(nvalue);
        if (instant)
            Do(instant);
    }

    public override void OnRegister()
    {
        base.OnRegister();

        procentage = StartProcentage;
        size = ProgressBarRect.sizeDelta;
        Do(true);

        UpdateSignal.AddListener(DoUpdate);
    }

    public override void OnRemove()
    {
        base.OnRemove();

        UpdateSignal.RemoveListener(DoUpdate);
    }

    private void DoUpdate(float delta)
    {
        Do();
    }

    private void Do(bool instant = false)
    {
        size.x = Mathf.Lerp(size.x, procentage * MaxSize, Time.deltaTime * Speed);
        if (instant)
            size.x = procentage * MaxSize;
        ProgressBarRect.sizeDelta = size;
    }
}