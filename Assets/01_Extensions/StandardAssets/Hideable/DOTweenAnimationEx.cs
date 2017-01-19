// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2015/03/12 15:55

using System;
using System.Collections.Generic;
using DG.Tweening.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if DOTWEEN_TMP

using TMPro;

#endif

#pragma warning disable 1591

namespace DG.Tweening
{
    /// <summary>
    /// Attach this to a GameObject to create a tween
    /// </summary>
    ///
    [System.Serializable]
    public class DOTweenAnimationEx
    {
        public GameObject DefaultTarget;
        public GameObject Target;

        public Transform transform;
        public GameObject gameObject;
        public bool hasOnComplete;
        public bool hasOnPlay;
        public bool hasOnStart;
        public bool hasOnStepComplete;
        public bool hasOnTweenCreated;
        public bool hasOnUpdate;
        public bool isSpeedBased;
        public UnityEvent onComplete;
        public UnityEvent onPlay;
        public UnityEvent onStart;
        public UnityEvent onStepComplete;
        public UnityEvent onTweenCreated;
        public UnityEvent onUpdate;
        public Tween tween;
        public UpdateType updateType;

        public float delay;
        public float duration = 1;
        public Ease easeType = Ease.OutQuad;
        public AnimationCurve easeCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public LoopType loopType = LoopType.Restart;
        public int loops = 1;
        public string id = "";
        public bool isRelative;
        public bool hasFrom;
        public bool isIndependentUpdate = false;
        public bool autoKill = true;

        public bool isActive = true;
        public bool isValid;
        public Component targetComponent;
        public DOTweenAnimationTypeEx animationType;
        public TargetType targetType;
        public TargetType forcedTargetType; // Used when choosing between multiple targets
        public bool autoPlay = true;
        public bool useTargetAsV3;

        public float endValueFloat;
        public Vector3 endValueV3;
        public Vector2 endValueV2;
        public Color endValueColor = new Color(1, 1, 1, 1);
        public string endValueString = "";
        public Rect endValueRect = new Rect(0, 0, 0, 0);
        public Transform endValueTransform;

        public float startValueFloat;
        public Vector3 startValueV3;
        public Vector2 startValueV2;
        public Color startValueColor = new Color(1, 1, 1, 1);
        public string startValueString = "";
        public Rect startValueRect = new Rect(0, 0, 0, 0);
        public Transform startValueTransform;

        public bool optionalBool0;
        public bool useStartAsEndValue;
        public bool useStartAsStartValue;
        public float optionalFloat0;
        public int optionalInt0;
        public RotateMode optionalRotationMode = RotateMode.Fast;
        public ScrambleMode optionalScrambleMode = ScrambleMode.None;
        public string optionalString;

        private int _playCount = -1; // Used when calling DOPlayNext

        #region Unity Methods

        public void Init()
        {
            LoadCurrentValues(ref useStartAsEndValue, ref endValueFloat, ref endValueV3, ref endValueV2, ref endValueColor, ref endValueString, ref endValueRect, ref endValueTransform);
            LoadCurrentValues(ref useStartAsStartValue, ref startValueFloat, ref startValueV3, ref startValueV2, ref startValueColor, ref startValueString, ref startValueRect, ref startValueTransform);
        }

        public void LoadCurrentValues(ref bool should, ref float floatValue, ref Vector3 ValueV3, ref Vector2 ValueV2, ref Color ValueColor, ref string ValueString, ref Rect ValueRect, ref Transform ValueTransform)
        {
            if (!should)
                return;
            switch (animationType)
            {
                case DOTweenAnimationTypeEx.None:
                    break;

                case DOTweenAnimationTypeEx.Move:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            ValueV3 = ((RectTransform)targetComponent).anchoredPosition3D;
                            break;

                        case TargetType.Transform:
                            ValueV3 = ((Transform)targetComponent).position;
                            break;

                        case TargetType.Rigidbody2D:
                            ValueV3 = ((Rigidbody2D)targetComponent).position;
                            break;

                        case TargetType.Rigidbody:
                            ValueV3 = ((Rigidbody)targetComponent).position;
                            break;
                    }

                    break;

                case DOTweenAnimationTypeEx.LocalMove:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            ValueV3 = ((RectTransform)targetComponent).anchoredPosition;
                            break;

                        case TargetType.Transform:
                            ValueV3 = ((Transform)targetComponent).localPosition;
                            break;
                    }

                    break;

                case DOTweenAnimationTypeEx.Rotate:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            ValueV3 = ((RectTransform)targetComponent).eulerAngles;
                            break;

                        case TargetType.Transform:
                            ValueV3 = ((Transform)targetComponent).eulerAngles;
                            break;
                    }

                    break;

                case DOTweenAnimationTypeEx.LocalRotate:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            ValueV3 = ((RectTransform)targetComponent).localEulerAngles;
                            break;

                        case TargetType.Transform:
                            ValueV3 = ((Transform)targetComponent).localEulerAngles;
                            break;
                    }

                    break;

                case DOTweenAnimationTypeEx.Scale:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            ValueV3 = ((RectTransform)targetComponent).localScale;
                            break;

                        case TargetType.Transform:
                            ValueV3 = ((Transform)targetComponent).localScale;
                            break;
                    }

                    break;

                case DOTweenAnimationTypeEx.Color:
                    isRelative = false;
                    switch (targetType)
                    {
                        case TargetType.SpriteRenderer:
                            ValueColor = ((SpriteRenderer)targetComponent).color;
                            break;

                        case TargetType.Renderer:
                            ValueColor = ((Renderer)targetComponent).material.color;
                            break;

                        case TargetType.Image:
                            ValueColor = ((Image)targetComponent).color;
                            break;

                        case TargetType.Text:
                            ValueColor = ((Text)targetComponent).color;
                            break;

                        case TargetType.Light:
                            ValueColor = ((Light)targetComponent).color;
                            break;
#if DOTWEEN_TMP
                            case TargetType.TextMeshProUGUI:
                                ValueColor = ((TextMeshProUGUI)targetComponent).color;
                                break;

                            case TargetType.TextMeshPro:
                                ValueColor = ((TextMeshPro)targetComponent).color;
                                break;
#endif
                    }

                    break;

                case DOTweenAnimationTypeEx.Fade:
                    isRelative = false;
                    switch (targetType)
                    {
                        case TargetType.SpriteRenderer:
                            ValueColor = ((SpriteRenderer)targetComponent).color;
                            floatValue = ValueColor.a;
                            break;

                        case TargetType.Renderer:
                            ValueColor = ((Renderer)targetComponent).material.color;
                            floatValue = ValueColor.a;
                            break;

                        case TargetType.Image:
                            ValueColor = ((Image)targetComponent).color;
                            floatValue = ValueColor.a;
                            break;

                        case TargetType.Text:
                            ValueColor = ((Text)targetComponent).color;
                            floatValue = ValueColor.a;
                            break;

                        case TargetType.Light:
                            ValueColor = ((Light)targetComponent).color;
                            floatValue = ValueColor.a;
                            break;

                        case TargetType.CanvasGroup:
                            floatValue = ((CanvasGroup)targetComponent).alpha;
                            break;

#if DOTWEEN_TMP
                            case TargetType.TextMeshProUGUI:
                                ValueColor = ((TextMeshProUGUI)target).color;
                                floatValue = ValueColor.a;
                                break;

                            case TargetType.TextMeshPro:
                                ValueColor = ((TextMeshPro)target).color;
                                floatValue = ValueColor.a;
                                break;
#endif
                    }

                    break;

                case DOTweenAnimationTypeEx.Text:
                    break;

                case DOTweenAnimationTypeEx.PunchPosition:
                    break;

                case DOTweenAnimationTypeEx.PunchRotation:
                    break;

                case DOTweenAnimationTypeEx.PunchScale:
                    break;

                case DOTweenAnimationTypeEx.ShakePosition:
                    break;

                case DOTweenAnimationTypeEx.ShakeRotation:
                    break;

                case DOTweenAnimationTypeEx.ShakeScale:
                    break;

                case DOTweenAnimationTypeEx.CameraAspect:
                    break;

                case DOTweenAnimationTypeEx.CameraBackgroundColor:
                    break;

                case DOTweenAnimationTypeEx.CameraFieldOfView:
                    break;

                case DOTweenAnimationTypeEx.CameraOrthoSize:
                    break;

                case DOTweenAnimationTypeEx.CameraPixelRect:
                    break;

                case DOTweenAnimationTypeEx.CameraRect:
                    break;

                case DOTweenAnimationTypeEx.UIWidthHeight:
                    ValueV2 = ((RectTransform)targetComponent).sizeDelta;

                    break;

                case DOTweenAnimationTypeEx.UIPivot:
                    ValueV2 = ((RectTransform)targetComponent).pivot;

                    break;

                case DOTweenAnimationTypeEx.UIAnchor:
                    ValueV2 = ((RectTransform)targetComponent).anchorMin;
                    ValueRect.x = ValueV2.x;
                    ValueRect.y = ValueV2.y;
                    ValueV2 = ((RectTransform)targetComponent).anchorMax;
                    ValueRect.width = ValueV2.x;
                    ValueRect.height = ValueV2.y;

                    break;

                default:
                    break;
            }
        }

        // Used also by DOTweenAnimationInspector when applying runtime changes and restarting
        public Tween CreateTween()
        {
            if (targetComponent == null)
            {
                Debug.LogWarning(string.Format("{0} :: This tween's target is NULL, because the animation was created with a DOTween Pro version older than 0.9.255. To fix this, exit Play mode then simply select this object, and it will update automatically", this.gameObject.name), this.gameObject);
                return null;
            }

            if (forcedTargetType != TargetType.Unset) targetType = forcedTargetType;
            if (targetType == TargetType.Unset)
            {
                // Legacy DOTweenAnimation (made with a version older than 0.9.450) without stored targetType > assign it now
                targetType = TypeToDOTargetType(targetComponent.GetType());
            }

            switch (animationType)
            {
                case DOTweenAnimationTypeEx.None:
                    break;

                case DOTweenAnimationTypeEx.Move:
                    if (useTargetAsV3)
                    {
                        isRelative = false;
                        if (endValueTransform == null)
                        {
                            Debug.LogWarning(string.Format("{0} :: This tween's TO target is NULL, a Vector3 of (0,0,0) will be used instead", this.gameObject.name), this.gameObject);
                            endValueV3 = Vector3.zero;
                        }
                        else
                        {
                            if (targetType == TargetType.RectTransform)
                            {
                                RectTransform endValueT = endValueTransform as RectTransform;
                                if (endValueT == null)
                                {
                                    Debug.LogWarning(string.Format("{0} :: This tween's TO target should be a RectTransform, a Vector3 of (0,0,0) will be used instead", this.gameObject.name), this.gameObject);
                                    endValueV3 = Vector3.zero;
                                }
                                else
                                {
                                    RectTransform rTarget = targetComponent as RectTransform;
                                    if (rTarget == null)
                                    {
                                        Debug.LogWarning(string.Format("{0} :: This tween's target and TO target are not of the same type. Please reassign the values", this.gameObject.name), this.gameObject);
                                    }
                                    else
                                    {
                                        // Problem: doesn't work inside Awake (ararargh!)
                                        endValueV3 = DOTweenUtils46.SwitchToRectTransform(endValueT, rTarget);
                                    }
                                }
                            }
                            else endValueV3 = endValueTransform.position;
                        }
                    }
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            tween = ((RectTransform)targetComponent).DOAnchorPos3D(endValueV3, duration, optionalBool0);
                            break;

                        case TargetType.Transform:
                            tween = ((Transform)targetComponent).DOMove(endValueV3, duration, optionalBool0);
                            break;

                        case TargetType.Rigidbody2D:
                            tween = ((Rigidbody2D)targetComponent).DOMove(endValueV3, duration, optionalBool0);
                            break;

                        case TargetType.Rigidbody:
                            tween = ((Rigidbody)targetComponent).DOMove(endValueV3, duration, optionalBool0);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.LocalMove:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            tween = ((RectTransform)targetComponent).DOAnchorPos(endValueV3, duration, optionalBool0);
                            break;

                        case TargetType.Transform:
                            tween = ((Transform)targetComponent).DOLocalMove(endValueV3, duration, optionalBool0);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.Rotate:
                    switch (targetType)
                    {
                        case TargetType.Transform:
                            tween = ((Transform)targetComponent).DORotate(endValueV3, duration, optionalRotationMode);
                            break;

                        case TargetType.Rigidbody2D:
                            tween = ((Rigidbody2D)targetComponent).DORotate(endValueFloat, duration);
                            break;

                        case TargetType.Rigidbody:
                            tween = ((Rigidbody)targetComponent).DORotate(endValueV3, duration, optionalRotationMode);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.LocalRotate:
                    tween = ((Transform)targetComponent).DOLocalRotate(endValueV3, duration, optionalRotationMode);
                    break;

                case DOTweenAnimationTypeEx.Scale:
                    switch (targetType)
                    {
#if DOTWEEN_TK2D
                case TargetType.tk2dTextMesh:
                    tween = ((tk2dTextMesh)target).DOScale(optionalBool0 ? new Vector3(endValueFloat, endValueFloat, endValueFloat) : endValueV3, duration);
                    break;

                case TargetType.tk2dBaseSprite:
                    tween = ((tk2dBaseSprite)target).DOScale(optionalBool0 ? new Vector3(endValueFloat, endValueFloat, endValueFloat) : endValueV3, duration);
                    break;
#endif
                        default:
                            tween = ((Transform)targetComponent).DOScale(optionalBool0 ? new Vector3(endValueFloat, endValueFloat, endValueFloat) : endValueV3, duration);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.UIWidthHeight:
                    tween = ((RectTransform)targetComponent).DOSizeDelta(optionalBool0 ? new Vector2(endValueFloat, endValueFloat) : endValueV2, duration);
                    break;

                case DOTweenAnimationTypeEx.UIPivot:
                    tween = ((RectTransform)targetComponent).DOPivot(endValueV2, duration);
                    break;

                case DOTweenAnimationTypeEx.UIAnchor:

                    endValueV2 = new Vector2(endValueRect.x, endValueRect.y);
                    tween = ((RectTransform)targetComponent).DOAnchorMin(endValueV2, duration);
                    endValueV2 = new Vector2(endValueRect.width, endValueRect.height);
                    tween = ((RectTransform)targetComponent).DOAnchorMax(endValueV2, duration);
                    break;

                case DOTweenAnimationTypeEx.Color:
                    isRelative = false;
                    switch (targetType)
                    {
                        case TargetType.SpriteRenderer:
                            tween = ((SpriteRenderer)targetComponent).DOColor(endValueColor, duration);
                            break;

                        case TargetType.Renderer:
                            tween = ((Renderer)targetComponent).material.DOColor(endValueColor, duration);
                            break;

                        case TargetType.Image:
                            tween = ((Image)targetComponent).DOColor(endValueColor, duration);
                            break;

                        case TargetType.Text:
                            tween = ((Text)targetComponent).DOColor(endValueColor, duration);
                            break;

                        case TargetType.Light:
                            tween = ((Light)targetComponent).DOColor(endValueColor, duration);
                            break;
#if DOTWEEN_TK2D
                case TargetType.tk2dTextMesh:
                    tween = ((tk2dTextMesh)target).DOColor(endValueColor, duration);
                    break;

                case TargetType.tk2dBaseSprite:
                    tween = ((tk2dBaseSprite)target).DOColor(endValueColor, duration);
                    break;
#endif
#if DOTWEEN_TMP
                        case TargetType.TextMeshProUGUI:
                            tween = ((TextMeshProUGUI)target).DOColor(endValueColor, duration);
                            break;

                        case TargetType.TextMeshPro:
                            tween = ((TextMeshPro)target).DOColor(endValueColor, duration);
                            break;
#endif
                    }
                    break;

                case DOTweenAnimationTypeEx.Fade:
                    isRelative = false;
                    switch (targetType)
                    {
                        case TargetType.SpriteRenderer:
                            tween = ((SpriteRenderer)targetComponent).DOFade(endValueFloat, duration);
                            break;

                        case TargetType.Renderer:
                            tween = ((Renderer)targetComponent).material.DOFade(endValueFloat, duration);
                            break;

                        case TargetType.Image:
                            tween = ((Image)targetComponent).DOFade(endValueFloat, duration);
                            break;

                        case TargetType.Text:
                            tween = ((Text)targetComponent).DOFade(endValueFloat, duration);
                            break;

                        case TargetType.Light:
                            tween = ((Light)targetComponent).DOIntensity(endValueFloat, duration);
                            break;

                        case TargetType.CanvasGroup:
                            tween = ((CanvasGroup)targetComponent).DOFade(endValueFloat, duration);
                            break;
#if DOTWEEN_TK2D
                case TargetType.tk2dTextMesh:
                    tween = ((tk2dTextMesh)target).DOFade(endValueFloat, duration);
                    break;

                case TargetType.tk2dBaseSprite:
                    tween = ((tk2dBaseSprite)target).DOFade(endValueFloat, duration);
                    break;
#endif
#if DOTWEEN_TMP
                        case TargetType.TextMeshProUGUI:
                            tween = ((TextMeshProUGUI)target).DOFade(endValueFloat, duration);
                            break;

                        case TargetType.TextMeshPro:
                            tween = ((TextMeshPro)target).DOFade(endValueFloat, duration);
                            break;
#endif
                    }
                    break;

                case DOTweenAnimationTypeEx.Text:
                    switch (targetType)
                    {
                        case TargetType.Text:
                            tween = ((Text)targetComponent).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
                            break;
#if DOTWEEN_TK2D
                case TargetType.tk2dTextMesh:
                    tween = ((tk2dTextMesh)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
                    break;
#endif
#if DOTWEEN_TMP
                        case TargetType.TextMeshProUGUI:
                            tween = ((TextMeshProUGUI)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
                            break;

                        case TargetType.TextMeshPro:
                            tween = ((TextMeshPro)target).DOText(endValueString, duration, optionalBool0, optionalScrambleMode, optionalString);
                            break;
#endif
                    }
                    break;

                case DOTweenAnimationTypeEx.PunchPosition:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            tween = ((RectTransform)targetComponent).DOPunchAnchorPos(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
                            break;

                        case TargetType.Transform:
                            tween = ((Transform)targetComponent).DOPunchPosition(endValueV3, duration, optionalInt0, optionalFloat0, optionalBool0);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.PunchScale:
                    tween = transform.DOPunchScale(endValueV3, duration, optionalInt0, optionalFloat0);
                    break;

                case DOTweenAnimationTypeEx.PunchRotation:
                    tween = transform.DOPunchRotation(endValueV3, duration, optionalInt0, optionalFloat0);
                    break;

                case DOTweenAnimationTypeEx.ShakePosition:
                    switch (targetType)
                    {
                        case TargetType.RectTransform:
                            tween = ((RectTransform)targetComponent).DOShakeAnchorPos(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
                            break;

                        case TargetType.Transform:
                            tween = ((Transform)targetComponent).DOShakePosition(duration, endValueV3, optionalInt0, optionalFloat0, optionalBool0);
                            break;
                    }
                    break;

                case DOTweenAnimationTypeEx.ShakeScale:
                    tween = transform.DOShakeScale(duration, endValueV3, optionalInt0, optionalFloat0);
                    break;

                case DOTweenAnimationTypeEx.ShakeRotation:
                    tween = transform.DOShakeRotation(duration, endValueV3, optionalInt0, optionalFloat0);
                    break;

                case DOTweenAnimationTypeEx.CameraAspect:
                    tween = ((Camera)targetComponent).DOAspect(endValueFloat, duration);
                    break;

                case DOTweenAnimationTypeEx.CameraBackgroundColor:
                    tween = ((Camera)targetComponent).DOColor(endValueColor, duration);
                    break;

                case DOTweenAnimationTypeEx.CameraFieldOfView:
                    tween = ((Camera)targetComponent).DOFieldOfView(endValueFloat, duration);
                    break;

                case DOTweenAnimationTypeEx.CameraOrthoSize:
                    tween = ((Camera)targetComponent).DOOrthoSize(endValueFloat, duration);
                    break;

                case DOTweenAnimationTypeEx.CameraPixelRect:
                    tween = ((Camera)targetComponent).DOPixelRect(endValueRect, duration);
                    break;

                case DOTweenAnimationTypeEx.CameraRect:
                    tween = ((Camera)targetComponent).DORect(endValueRect, duration);
                    break;
            }

            if (tween == null) return null;

            tween.SetRelative(isRelative);

            tween.SetTarget(this.gameObject).SetDelay(delay).SetLoops(loops, loopType).SetAutoKill(autoKill)
                .OnKill(() => tween = null);
            if (isSpeedBased) tween.SetSpeedBased();
            if (easeType == Ease.INTERNAL_Custom) tween.SetEase(easeCurve);
            else tween.SetEase(easeType);
            if (!string.IsNullOrEmpty(id)) tween.SetId(id);
            tween.SetUpdate(isIndependentUpdate);

            if (hasOnStart)
            {
                if (onStart != null) tween.OnStart(onStart.Invoke);
            }
            else onStart = null;
            if (hasOnPlay)
            {
                if (onPlay != null) tween.OnPlay(onPlay.Invoke);
            }
            else onPlay = null;
            if (hasOnUpdate)
            {
                if (onUpdate != null) tween.OnUpdate(onUpdate.Invoke);
            }
            else onUpdate = null;
            if (hasOnStepComplete)
            {
                if (onStepComplete != null) tween.OnStepComplete(onStepComplete.Invoke);
            }
            else onStepComplete = null;
            if (hasOnComplete)
            {
                if (onComplete != null) tween.OnComplete(onComplete.Invoke);
            }
            else onComplete = null;

            if (autoPlay) tween.Play();
            else tween.Pause();

            if (hasOnTweenCreated && onTweenCreated != null) onTweenCreated.Invoke();
            return tween;
        }

        #endregion Unity Methods

        #region Public Methods

        // These methods are here so they can be called directly via Unity's UGUI event system

        public void DOPlay()
        {
            DOTween.Play(this.gameObject);
        }

        public void DOPlayBackwards()
        {
            DOTween.PlayBackwards(this.gameObject);
        }

        public void DOPlayForward()
        {
            DOTween.PlayForward(this.gameObject);
        }

        public void DOPause()
        {
            DOTween.Pause(this.gameObject);
        }

        public void DOTogglePause()
        {
            DOTween.TogglePause(this.gameObject);
        }

        public void DORewind()
        {
            _playCount = -1;
            // Rewind using Components order (in case there are multiple animations on the same property)
            DOTweenAnimation[] anims = this.gameObject.GetComponents<DOTweenAnimation>();
            for (int i = anims.Length - 1; i > -1; --i)
            {
                Tween t = anims[i].tween;
                if (t != null && t.IsInitialized()) anims[i].tween.Rewind();
            }
            // DOTween.Rewind(this.gameObject);
        }

        /// <summary>
        /// Restarts the tween
        /// </summary>
        /// <param name="fromHere">If TRUE, re-evaluates the tween's start and end values from its current position.
        /// Set it to TRUE when spawning the same DOTweenAnimation in different positions (like when using a pooling system)</param>
        public void DORestart(bool fromHere = false)
        {
            _playCount = -1;
            if (tween == null)
            {
                if (Debugger.logPriority > 1) Debugger.LogNullTween(tween); return;
            }
            if (fromHere && isRelative) ReEvaluateRelativeTween();
            DOTween.Restart(this.gameObject);
        }

        public void DOComplete()
        {
            DOTween.Complete(this.gameObject);
        }

        public void DOKill()
        {
            DOTween.Kill(this.gameObject);
            tween = null;
        }

        #region Specifics

        public void DOPlayById(string id)
        {
            DOTween.Play(this.gameObject, id);
        }

        public void DOPlayAllById(string id)
        {
            DOTween.Play(id);
        }

        public void DOPlayBackwardsById(string id)
        {
            DOTween.PlayBackwards(this.gameObject, id);
        }

        public void DOPlayBackwardsAllById(string id)
        {
            DOTween.PlayBackwards(id);
        }

        public void DOPlayForwardById(string id)
        {
            DOTween.PlayForward(this.gameObject, id);
        }

        public void DOPlayForwardAllById(string id)
        {
            DOTween.PlayForward(id);
        }

        public void DORestartById(string id)
        {
            _playCount = -1;
            DOTween.Restart(this.gameObject, id);
        }

        public void DORestartAllById(string id)
        {
            _playCount = -1;
            DOTween.Restart(id);
        }

        #endregion Specifics

        #region Internal Static Helpers (also used by Inspector)

        public static TargetType TypeToDOTargetType(Type t)
        {
            string str = t.ToString();
            int dotIndex = str.LastIndexOf(".");
            if (dotIndex != -1) str = str.Substring(dotIndex + 1);
            if (str.IndexOf("Renderer") != -1 && (str != "SpriteRenderer")) str = "Renderer";
            return (TargetType)Enum.Parse(typeof(TargetType), str);
        }

        #endregion Internal Static Helpers (also used by Inspector)

        #endregion Public Methods

        #region Private

        // Re-evaluate relative position of path
        private void ReEvaluateRelativeTween()
        {
            if (animationType == DOTweenAnimationTypeEx.Move)
            {
                ((Tweener)tween).ChangeEndValue(transform.position + endValueV3, true);
            }
            else if (animationType == DOTweenAnimationTypeEx.LocalMove)
            {
                ((Tweener)tween).ChangeEndValue(transform.localPosition + endValueV3, true);
            }
        }

        #endregion Private
    }
}