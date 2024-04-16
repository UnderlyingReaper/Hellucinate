using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashLight : MonoBehaviour
{
    public DetectLight detectLight;

    [Header("General")]
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

    float _timePassedPerBatter;
    bool _isFlickering;



    void Start()
    {
        StartCoroutine(unsatbleLightAnim());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isEnabled && currBatteries > 0) // Turn on
        {
            ChangeIntensityOfLight(maxIntensity, true);
            PlayPressSound();
        }
        else if(Input.GetKeyDown(KeyCode.F) && isEnabled) // Turn off
        {
            ChangeIntensityOfLight(minIntensity, false);
            PlayPressSound();
        }

        if(currBatteries == 0 && isEnabled) // If no more batteries, kill light
            ChangeIntensityOfLight(minIntensity, false);

        if(isEnabled) ReduceBattery(); // reduce battery if light is on
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
                    DOVirtual.Float(lightSources[index].intensity, Random.Range(flickerMinValue, 1), flickeringSpeed, value => { lightSources[index].intensity = value; });
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
        for(int i = 0; i < lightSources.Length; i++)
        {
            int index = i;
            DOVirtual.Float(lightSources[index].intensity, minIntensity, delay, value => {lightSources[index].intensity = value; }); // Low
        }
        yield return new WaitForSeconds(delay); // Delay

        for(int i = 0; i < lightSources.Length; i++)
        {
            int index = i;
            DOVirtual.Float(lightSources[index].intensity, maxIntensity, delay, value => {lightSources[index].intensity = value; }); // High
        }
        yield return new WaitForSeconds(delay); // Delay

        for(int i = 0; i < lightSources.Length; i++)
        {
            int index = i;
            DOVirtual.Float(lightSources[index].intensity, minIntensity, delay, value => {lightSources[index].intensity = value; }); // Low
        }
        yield return new WaitForSeconds(delay); // Delay

        for(int i = 0; i < lightSources.Length; i++)
        {
            int index = i;
            DOVirtual.Float(lightSources[index].intensity, maxIntensity, delay, value => {lightSources[index].intensity = value; }); // High
        }
        _isFlickering = false;
    }


    void PlayPressSound()
    {
        audio_Source.volume = Random.Range(0.8f, 1.2f);
        audio_Source.pitch = Random.Range(0.8f, 1.2f);

        audio_Source.PlayOneShot(press_Clip);
    }
}
