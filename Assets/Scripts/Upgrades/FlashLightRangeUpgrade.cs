using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashLightRangeUpgrade : MonoBehaviour, IInteractible
{
    public bool allowInteract;

    [Tooltip("+Value: +Light Range \n-Value: -Light Range")]
    public float rangeIncreaseAmt;

    [Tooltip("+Value: +Per Battery Life \n-Value: -Per Battery Life")]
    public float timeIncreaseAmt;

    [Tooltip("+Value: +Light Intensity \n-Value: -Light Intensity")]
    public float intensityIncreaseAmt;

    public AudioSource source;
    public CanvasGroup canvas;

    public TextMeshProUGUI rangeDisplay;
    public TextMeshProUGUI effeciencyDisplay;
    public TextMeshProUGUI intensityDisplay;


    FlashLight _flashLight;
    Material _material;
    Light2D[] _lightSources;



    void Start()
    {
        _flashLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FlashLight>();
        _lightSources = _flashLight.lightSources;

        _material = GetComponentInChildren<SpriteRenderer>().material;

        if(rangeIncreaseAmt > 0)
        {
            rangeDisplay.text += $" +{rangeIncreaseAmt}";
            rangeDisplay.color = Color.green;
        }
        else if(rangeIncreaseAmt < 0)
        {
            rangeDisplay.text += $" -{rangeIncreaseAmt}";
            rangeDisplay.color = Color.red;
        }
        else rangeDisplay.gameObject.SetActive(false);


        if(timeIncreaseAmt > 0)
        {
            effeciencyDisplay.text += $" +{timeIncreaseAmt}";
            effeciencyDisplay.color = Color.green;
        }
        else if(timeIncreaseAmt < 0)
        {
            effeciencyDisplay.text += $" -{timeIncreaseAmt}";
            effeciencyDisplay.color = Color.red;
        }
        else effeciencyDisplay.gameObject.SetActive(false);


        if(intensityIncreaseAmt > 0)
        {
            intensityDisplay.text += $" +{intensityIncreaseAmt}";
            intensityDisplay.color = Color.green;
        }
        else if(intensityIncreaseAmt < 0)
        {
            intensityDisplay.text += $" -{intensityIncreaseAmt}";
            intensityDisplay.color = Color.red;
        }
        else intensityDisplay.gameObject.SetActive(false);

        canvas.alpha = 0;
    }

    public void HideCanvas()
    {
        canvas.DOFade(0, 1f);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(allowInteract)
        {
            allowInteract = false;
            source.PlayOneShot(source.clip);

            _lightSources[1].pointLightOuterRadius += rangeIncreaseAmt;

            _flashLight.perbatteryTime += timeIncreaseAmt;

            _flashLight.maxIntensity += intensityIncreaseAmt;
            _flashLight.flickerMinValue += intensityIncreaseAmt * 0.8f;


            if(_material != null) DOVirtual.Float(_material.GetFloat("_Fade"), 0, 2, value => {_material.SetFloat("_Fade", value); });

            canvas.DOFade(0, 1f);

            Destroy(gameObject, 2.1f);
        }
    }

    public void OnInteractKeyDown()
    {
    }

    public void OnInteractKeyUp()
    {
    }

    public void ShowCanvas()
    {
        if(allowInteract) canvas.DOFade(1, 1f);
    }
}
