using System.Collections;
using UnityEngine;

public class FollowUI : PooledBehaviour
{
    [SerializeField]
    [ReadOnly]
    private Transform target;

    private RectTransform rect;
    public float Speed = 5f;
    public ImageColorFade Fader;

    [Inject]
    public UpdateSignal UpdateSignal { get; set; }

    public override void OnRegister()
    {
        base.OnRegister();

        UpdateSignal.AddListener(DoUpdate);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UpdateSignal.RemoveListener(DoUpdate);
    }

    public void Set(Transform trans)
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        target = trans;

        if (target != null)
        {
            DoInstant();
            Fader.Show();
        }
        else
        {
            Fader.Hide(false, Despawn);
        }
    }

    private void DoUpdate(float delta)
    {
        Do(delta);
    }

    private void Do(float delta)
    {
        if (target == null)
            return;
        if (Speed <= 0f)
            DoInstant();
        else
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, Camera.main.WorldToScreenPoint(target.position), delta * Speed);
    }

    private void DoInstant()
    {
        rect.anchoredPosition = Camera.main.WorldToScreenPoint(target.position);
    }
}