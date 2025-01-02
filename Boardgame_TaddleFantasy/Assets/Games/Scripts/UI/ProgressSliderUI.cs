using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ProgressSliderUI : MonoBehaviour
{
    [SerializeField]
    protected Slider sliderProgress;
    [SerializeField]
    protected GameObject sliderHandler;
    [SerializeField]
    protected TextMeshProUGUI txtProgress;

    private void OnEnable()
    {
        if(sliderProgress == null)
        {
            Debug.Log("Slider null, you should check instead of using this cover");
            sliderProgress = GetComponent<Slider>();
            if (sliderProgress != null)
                return;
        }
        if (sliderProgress == null)
        {
            Debug.Log("Slider null, you should check instead of using this cover");
            sliderProgress = GetComponentInChildren<Slider>();
            if (sliderProgress != null)
                return;
        }
        if (sliderProgress == null)
            Debug.LogError("Slider NOT ASSIGN, FIX IMMEDIATELY " + this.transform.parent.name);
    }
    public virtual void ShowHandler(bool isShow)
    {
        if(sliderHandler!= null)
        {
            sliderHandler.SetActive(isShow);
        }
    }
    public virtual void UpdateFill(float current, float total)
    {
        //float fill = current / total;
        sliderProgress.maxValue = total;
        this.sliderProgress.value = current;
        this.UpdateProgress($"{current}/{total}");
    }
    public virtual Tween AnimationFill(float current, float total, float duration = 0.5f)
    {
        sliderProgress.maxValue = total;
        return sliderProgress.DOValue(current, duration);
    }
    public virtual void UpdateFillAndProgress(float current, float total, string progress)
    {
        sliderProgress.maxValue = total;
        this.sliderProgress.value = current;
        this.UpdateProgress(progress);
    }

    public virtual void UpdateFill(float fill)
    {
        this.sliderProgress.value = fill;
    }

    public virtual void UpdateProgress(string value)
    {
        if(txtProgress != null)
            this.txtProgress.text = value;
    }
}
