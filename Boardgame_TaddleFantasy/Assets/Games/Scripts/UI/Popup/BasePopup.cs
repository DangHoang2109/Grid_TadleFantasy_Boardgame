using DG.Tweening.Core.Easing;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasePopup : MonoBehaviour
{
    [Tooltip("should be at root object")]
    [SerializeField]
    protected CanvasGroup canvasGroup;
    [SerializeField]
    protected Transform panel;

    protected float transitionTime = 0.2f;

    #region event
    public System.Action OnShowing;
    public System.Action<BasePopup> OnShowed;
    public System.Action OnClosing;
    public System.Action OnClosed;
    #endregion

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
    }
#endif

    public virtual void OnShow(System.Action callback = null)
    {
        this.gameObject.SetActive(true);
        this.AnimationShow();
        this.OnShowing?.Invoke();
    }

    protected virtual void AnimationShow()
    {
        this.panel.localScale = Vector3.zero;
        if (this.canvasGroup != null)
        {
            this.canvasGroup.alpha = 0;

        }
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(1f, this.transitionTime).SetEase(Ease.OutBack).OnComplete(this.OnCompleteShow));
        if (this.canvasGroup != null)
        {
            seq.Join(this.canvasGroup.DOFade(1, this.transitionTime));
        }
    }
    protected virtual void OnCompleteShow()
    {
        this.OnShowed?.Invoke(this);
        OnShowed = null;
    }
    public virtual void OnHide()
    {
        this.AnimationHide();
    }
    protected virtual void AnimationHide()
    {
        Sequence seq = DOTween.Sequence();
        seq.Join(this.panel.DOScale(0.0f, this.transitionTime).SetEase(Ease.Linear).OnComplete(this.OnCompleteHide));
        if (this.canvasGroup != null)
        {
            seq.Join(this.canvasGroup.DOFade(0, this.transitionTime));
        }
    }
    protected virtual void OnCompleteHide()
    {
        this.gameObject.SetActive(false);

        this.OnClosed?.Invoke();
        this.OnClosed = null;
    }
    public virtual void OnCloseDialog()
    {
        this.OnClosing?.Invoke();
        this.OnClosing = null;
        OnHide();
        PopupManager.Instance.OnAPopupHide(this);
    }
    public virtual void ClickClose()
    {
        this.OnCloseDialog();
    }

}
