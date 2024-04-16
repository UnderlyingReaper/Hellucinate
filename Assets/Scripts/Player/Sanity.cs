using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Sanity : MonoBehaviour
{
    [Header("Sanity")]
    public DetectLight detectLight;
    public float sanityGainSpeedMp;
    public float sanityLossSpeedMp;

    [Header("Sanity Effects")]
    public Volume postProcessingVolume;
    [SerializeField][Range(0f, 100f)] float sanity;
    public AudioSource heartBeat_Source;
    public AudioSource breathing_Source;
    public float minDelay, maxDelay;
    public float minGrain, maxGrain;

    
    Vignette _vignette;
    FilmGrain _filmGrain;



    void Start()
    {
        postProcessingVolume.profile.TryGet(out _vignette);
        postProcessingVolume.profile.TryGet(out _filmGrain);

        StartCoroutine(SanityHeartBeatEffect());

        detectLight.LightDetectionResult += LightDetectionResult;
    }

    void Update()
    {
        SanityEffects();
    }

    public void GainSanity(float amount)
    {
        if(sanity < 100)
            sanity += amount * sanityGainSpeedMp;
    }
    public void LoseSanity(float amount)
    {
        if(sanity > 0)
            sanity -= amount * sanityLossSpeedMp;
    }

    public void RemoveSanity(float amount)
    {
        if(sanity > 0)
            sanity -= amount;
    }

    public void LightDetectionResult(object sender, DetectLight.LightDetectionResultArgs e)
    {
        if(e.isInLight) GainSanity(Time.deltaTime);
        else LoseSanity(Time.deltaTime);
    }

    public IEnumerator SanityHeartBeatEffect()
    {
        while(true)
        {
            float delayToUse = 1 + ((maxDelay - minDelay)*(100 - sanity))/100;

            yield return new WaitForSeconds(delayToUse);

            // Heartbeat
            float volToUse = 0.08f + (0.4f - 0.08f) * (1 - (sanity/100));
            heartBeat_Source.volume = Random.Range(volToUse - 0.1f, volToUse + 0.1f);
            if(heartBeat_Source.volume <= 0) heartBeat_Source.volume = 0.08f;

            heartBeat_Source.pitch = Random.Range(0.9f, 1.1f);
            heartBeat_Source.PlayOneShot(heartBeat_Source.clip);
            // Heartbeat
        }
    }

    public void SanityEffects()
    {
        // Breathing
        if(sanity <= 50) breathing_Source.volume = 0.1f + (0.8f - 0.1f)*(1 - (sanity/100));
        else DOVirtual.Float(breathing_Source.volume, 0, 0.2f, value => { breathing_Source.volume = value; });
        // Breathing

        // Vignette
        float desiredVignetteIntensity = 0.3f + (1f - 0.3f) * (1 - (sanity/100));
        DOVirtual.Float(_vignette.intensity.value, desiredVignetteIntensity, 0.1f, value => { _vignette.intensity.value = value; });
        // Vignette

        // FilmGrain
        float desiredFilmGrainIntensity = minGrain + (maxGrain - minGrain) * (1 - (sanity/100));
        DOVirtual.Float(_filmGrain.intensity.value, desiredFilmGrainIntensity, 0.1f, value => { _filmGrain.intensity.value = value; });
        // FilmGrain
    }
}
