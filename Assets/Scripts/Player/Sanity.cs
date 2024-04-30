using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Sanity : MonoBehaviour
{
    [Header("Sanity")]
    public DetectLight detectLight;
    public float sanity;
    public float sanityGainSpeedMp;
    public float sanityLossSpeedMp;

    [Header("Sanity Effects")]
    public Volume postProcessingVolume;
    public AudioSource heartBeat_Source;
    public AudioSource breathing_Source;

    [Header("Kill Player")]
    public CanvasGroup fade;
    public TextMeshProUGUI deathTxt;
    Player_Movement _pm;

    
    Vignette _vignette;
    DepthOfField _dof;
    FilmGrain _filmGrain;



    void Start()
    {
        postProcessingVolume.profile.TryGet(out _vignette);
        postProcessingVolume.profile.TryGet(out _filmGrain);
        postProcessingVolume.profile.TryGet(out _dof);

        _pm = GetComponent<Player_Movement>();

        StartCoroutine(SanityHeartBeatEffect());

        detectLight.LightDetectionResult += LightDetectionResult;
    }

    void Update()
    {
        SanityEffects();

        if(sanity == 0)
        {
            StopCoroutine(SanityHeartBeatEffect());
            //StartCoroutine(KillPlayer());
        }
    }

    public void GainSanity(float amount, float mp)
    {
        if(sanity < 100)
            sanity += amount * mp;

        if(sanity > 100) sanity = 100;
    }
    public void LoseSanity(float amount, float mp)
    {
        if(sanity > 0)
            sanity -= amount * mp;

        if(sanity < 0) sanity = 0;
    }

    void LightDetectionResult(object sender, DetectLight.LightDetectionResultArgs e)
    {
        if(e.isInLight) GainSanity(Time.deltaTime, sanityGainSpeedMp);
        else LoseSanity(Time.deltaTime, sanityLossSpeedMp);
    }

    IEnumerator SanityHeartBeatEffect()
    {
        while(true)
        {
            float delayToUse = 1 + ((1 - 0.5f)*(sanity))/100;

            yield return new WaitForSeconds(delayToUse);

            // Heartbeat
            float volToUse = 0.08f + (0.4f - 0.08f) * (1 - (sanity/100));

            heartBeat_Source.volume = UnityEngine.Random.Range(volToUse - 0.1f, volToUse + 0.1f);
            if(heartBeat_Source.volume <= 0) heartBeat_Source.volume = 0.08f;
            heartBeat_Source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);

            heartBeat_Source.PlayOneShot(heartBeat_Source.clip);
            // Heartbeat
        }
    }
    void SanityEffects()
    {
        // Breathing
        if(sanity <= 50) breathing_Source.volume = 0.1f + (0.8f - 0.1f)*(1 - (sanity - 50/100));
        else DOVirtual.Float(breathing_Source.volume, 0, 0.2f, value => { breathing_Source.volume = value; });
        // Breathing

        // Depth of field
        if(sanity <= 25) _dof.focusDistance.value = 5 - (4 *(50 - sanity)/50);
        else DOVirtual.Float(_dof.focusDistance.value, 5, 2, value => { _dof.focusDistance.value = value; });
        // Depth of field

        // Vignette
        float desiredVignetteIntensity = 0.3f + (1f - 0.3f) * (1 - (sanity/100));
        DOVirtual.Float(_vignette.intensity.value, desiredVignetteIntensity, 0.1f, value => { _vignette.intensity.value = value; });
        // Vignette

        // FilmGrain
        float desiredFilmGrainIntensity = 0.1f + (0.3f - 0.1f) * (1 - (sanity/100));
        DOVirtual.Float(_filmGrain.intensity.value, desiredFilmGrainIntensity, 0.1f, value => { _filmGrain.intensity.value = value; });
        // FilmGrain
    }

    IEnumerator KillPlayer() // Its better to have a seperate script to handle players death.
    {
        _pm.enabled = false;
        DOVirtual.Float(_dof.focusDistance.value, 0, 1, value => { _dof.focusDistance.value = value; });
        deathTxt.text = "Beware the shadows, for they feed upon your mind. Stay bathed in light to safeguard your sanity.";
        fade.DOFade(1, 1);

        yield return new WaitForSeconds(3);

        deathTxt.GetComponent<CanvasGroup>().DOFade(0, 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
