using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    public DetectLight detectLight;

    [Header("General")]
    public Slider slider;
    public Ease easeType;
    public AudioSource audio_Source;
    public AudioClip press_Clip;
    public Light2D[] lightSources;
    public float minIntensity = 0, maxIntensity = 1, duration = 0.5f;
    public bool isEnabled;

    [Header("Flickering Animation")]
    public float flickeringSpeed;
    public float flickerMinValue;

    [Header("Batter System")]
    public float perbatteryTime;
    public float currBatteries;
    public float flashesDelay;
    public float flickerAmt;

    float _timePassedPerBatter;

    CanvasGroup _sliderCg;
    Vector3 _orgSliderPos;
    RectTransform _sliderTransform;

    bool _isFlickering;
    PlayerInputManager _playerInputManager;



    void Start()
    {
        StartCoroutine(unsatbleLightAnim());
        _sliderCg = slider.GetComponent<CanvasGroup>();
        _sliderTransform = slider.GetComponent<RectTransform>();
        _orgSliderPos = _sliderTransform.anchoredPosition;

        _playerInputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Flashlight.performed += PerformAction;

        _sliderTransform.anchoredPosition = new Vector2(0, 0);
        _sliderCg.alpha = 0;
    }

    void Update()
    {
        if(isEnabled && !_isFlickering) ReduceBattery();
    }

    public void PerformAction(InputAction.CallbackContext context)
    {
        if(!isEnabled && currBatteries > 0) // Turn on
        {
            ChangeIntensityOfLight(maxIntensity, true);
            _sliderTransform.DOAnchorPosX(_orgSliderPos.x, 0.3f).SetEase(easeType);
            _sliderCg.DOFade(1, 0.3f);
            PlayPressSound();
        }
        else if(isEnabled) // Turn off
        {
            ChangeIntensityOfLight(minIntensity, false);
            _sliderTransform.DOAnchorPosX(0, 0.3f).SetEase(easeType);
            _sliderCg.DOFade(0, 0.3f);
            PlayPressSound();
        }

        if(currBatteries == 0 && isEnabled) // If no more batteries, kill light
        {
            ChangeIntensityOfLight(minIntensity, false);
            _sliderTransform.DOAnchorPosX(0, 0.3f).SetEase(easeType);
            _sliderCg.DOFade(0, 0.3f);
        }
    }


    public IEnumerator unsatbleLightAnim()
    {
        while(true)
        {
            if(isEnabled && !_isFlickering)
            {
                for(int i = 0; i < lightSources.Length; i++)
                {
                    int index = i;
                    DOVirtual.Float(lightSources[index].intensity, UnityEngine.Random.Range(flickerMinValue, 1), flickeringSpeed, value => { lightSources[index].intensity = value; });
                }
            }

            yield return new WaitForSeconds(flickeringSpeed);
        }
    }

    public void ChangeIntensityOfLight(float desiredInternsity, bool isOn)
    {
        isEnabled = isOn;
        
        for(int i = 0; i < lightSources.Length; i++)
        {
            int index = i;
            DOVirtual.Float(lightSources[index].intensity, desiredInternsity, duration, value => {lightSources[index].intensity = value; });
        }

        detectLight.isFlashLightOn = isEnabled;
    }

    public void ReduceBattery()
    {
        _timePassedPerBatter += Time.deltaTime;
        _timePassedPerBatter = Math.Clamp(_timePassedPerBatter, 0, perbatteryTime);

        slider.value = 15 - _timePassedPerBatter;

        if(_timePassedPerBatter >= perbatteryTime)
        {
            StartCoroutine(FlickerLights(flashesDelay));
            _timePassedPerBatter = 0;
            currBatteries--;
        } 
    }
    public IEnumerator FlickerLights(float delay)
    {
        _isFlickering = true;

        for(int i = 0; i < flickerAmt; i++)
        {
            for(int j = 0; j < lightSources.Length; j++)
            {
                int index = j;
                if(i%2 == 0)
                    DOVirtual.Float(lightSources[index].intensity, minIntensity, delay, value => {lightSources[index].intensity = value; });
                else
                    DOVirtual.Float(lightSources[index].intensity, maxIntensity, delay, value => {lightSources[index].intensity = value; });
            }

            yield return new WaitForSeconds(delay); // Delay  
        }

        _isFlickering = false;
    }

    void PlayPressSound()
    {
        audio_Source.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        audio_Source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        audio_Source.PlayOneShot(press_Clip);
    }
}
