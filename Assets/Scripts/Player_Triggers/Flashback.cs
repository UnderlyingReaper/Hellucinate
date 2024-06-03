using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Flashback : MonoBehaviour
{
    public Volume volume;
    public float minSat = -100;
    public float minLensDistort = -0.4f;
    public CanvasGroup flashCanvas;
    public float fadeInTime, fadeOutTime;
    public AnimationCurve FadeInCurve, FadeOutCurve;

    public FlashbackTrigger[] flashBackTriggers;


    Rigidbody2D _rb;
    float _satOrgVal;
    ColorAdjustments _colorAdjustments;
    MotionBlur _motionBlur;
    LensDistortion _lensDistortion;
    ChromaticAberration _chromaticAberration;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        volume.profile.TryGet(out _colorAdjustments);
        volume.profile.TryGet(out _motionBlur);
        volume.profile.TryGet(out _lensDistortion);
        volume.profile.TryGet(out _chromaticAberration);

        _satOrgVal = _colorAdjustments.saturation.value;

        for(int i = 0; i < flashBackTriggers.Length; i++)
        {
            flashBackTriggers[i].OnFlashTrigger += OnFlashTrigger;
        }
    }

    public void OnFlashTrigger(object sender, FlashbackTrigger.FlashTriggerInfo e)
    {
        if(e.isStart) StartCoroutine(StartFlashBack());
        else if(!e.isStart) StartCoroutine(StopFlashBack());
    }

    IEnumerator StartFlashBack()
    {
        flashCanvas.DOFade(1, fadeInTime).SetEase(FadeInCurve);
        DOVirtual.Float(Camera.main.fieldOfView, 52, fadeInTime, value => { Camera.main.fieldOfView = value; });
        yield return new WaitForSeconds(fadeInTime);


        _colorAdjustments.saturation.value = minSat;
        _lensDistortion.intensity.value = minLensDistort;
        _chromaticAberration.intensity.value = 1;
        _motionBlur.intensity.value = 1;
        _rb.drag = 50;


        flashCanvas.DOFade(0, fadeOutTime).SetEase(FadeOutCurve);
        yield return new WaitForSeconds(fadeOutTime);
    }
    IEnumerator StopFlashBack()
    {
        flashCanvas.DOFade(1, fadeInTime).SetEase(FadeInCurve);
        DOVirtual.Float(Camera.main.fieldOfView, 50, fadeInTime, value => { Camera.main.fieldOfView = value; });
        yield return new WaitForSeconds(fadeInTime);


        _colorAdjustments.saturation.value = _satOrgVal;
        _lensDistortion.intensity.value = -0.1f;
        _chromaticAberration.intensity.value = 0;
        _motionBlur.intensity.value = 0;
        _rb.drag = 0;


        flashCanvas.DOFade(0, fadeOutTime).SetEase(FadeOutCurve);
        yield return new WaitForSeconds(fadeOutTime);
    }
}
