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
    public AnimationCurve playerWalkSpeedCurve;

    [Space(20)]

    public AnimationCurve heartBeatCurve;
    public AnimationCurve delayToUseCurve;

    [Space(20)]

    public AnimationCurve breathVolCurve;
    public AnimationCurve focusDistanceCurve;
    public AnimationCurve vignetteCurve;
    public AnimationCurve filmGrainCurve;

    [Space(20)]

    public AudioSource heartBeat_Source;
    public AudioSource breathing_Source;

    [Header("Kill Player")]
    public bool killPlayer;
    public CanvasGroup fade;
    public TextMeshProUGUI deathTxt;
    Player_Movement _pm;

    
    Vignette _vignette;
    DepthOfField _dof;
    FilmGrain _filmGrain;
    CanvasGroup _deathTxtCanvasGroup;



    void Start()
    {
        postProcessingVolume.profile.TryGet(out _vignette);
        postProcessingVolume.profile.TryGet(out _filmGrain);
        postProcessingVolume.profile.TryGet(out _dof);

        _pm = GetComponent<Player_Movement>();
        _deathTxtCanvasGroup = deathTxt.GetComponent<CanvasGroup>();

        StartCoroutine(SanityHeartBeatEffect());

        detectLight.LightDetectionResult += LightDetectionResult;
    }

    void Update()
    {
        if(!killPlayer) SanityEffects();

        if(sanity == 0)
        {
            killPlayer = true;
            DOVirtual.Float(breathing_Source.volume, 0, 2, value => { breathing_Source.volume = value; });
            DOVirtual.Float(heartBeat_Source.volume, 0, 2, value => { heartBeat_Source.volume = value; });
            StartCoroutine(KillPlayer());
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
            float delayToUse = delayToUseCurve.Evaluate(sanity);

            yield return new WaitForSeconds(delayToUse);

            // Heartbeat
            if(!killPlayer)
            {
                float volToUse = heartBeatCurve.Evaluate(sanity);

                heartBeat_Source.volume = UnityEngine.Random.Range(volToUse - 0.2f, volToUse + 0.2f);
                if(heartBeat_Source.volume <= 0) heartBeat_Source.volume = 0.08f;
                heartBeat_Source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            }

            heartBeat_Source.PlayOneShot(heartBeat_Source.clip);
            // Heartbeat
        }
    }
    void SanityEffects()
    {
        // Breathing
        DOVirtual.Float(breathing_Source.volume, breathVolCurve.Evaluate(sanity), 0.2f, value => { breathing_Source.volume = value; });
        // Breathing

        // Depth of field
        DOVirtual.Float(_dof.focusDistance.value, focusDistanceCurve.Evaluate(sanity), 0.2f, value => { _dof.focusDistance.value = value; }); 
        // Depth of field

        // Player walk speed
        _pm.ManipulatePlayerSpeed(playerWalkSpeedCurve.Evaluate(sanity));
        // Player walk speed

        // Vignette
        DOVirtual.Float(_vignette.intensity.value, vignetteCurve.Evaluate(sanity), 0.2f, value => { _vignette.intensity.value = value; }); 
        // Vignette

        // FilmGrain
        DOVirtual.Float(_filmGrain.intensity.value, filmGrainCurve.Evaluate(sanity), 0.2f, value => { _filmGrain.intensity.value = value; });
        // FilmGrain
    }

    IEnumerator KillPlayer() // Its better to have a seperate script to handle players death.
    {
        _pm.allow = false;
        DOVirtual.Float(_dof.focusDistance.value, 0, 2, value => { _dof.focusDistance.value = value; });
        deathTxt.text = "Beware the shadows, for they feed upon your mind. Stay bathed in light to safeguard your sanity.";

        yield return new WaitForSeconds(3);

        fade.DOFade(1, 2);

        yield return new WaitForSeconds(3);

        _deathTxtCanvasGroup.DOFade(1, 2);

        yield return new WaitForSeconds(5);

        _deathTxtCanvasGroup.DOFade(0, 2);

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
