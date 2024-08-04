using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashLightRangeUpgrade : MonoBehaviour, IInteractible
{
    public bool allowInteract;
    public int increaseAmt;
    public AudioSource source;
    public CanvasGroup canvas;


    FlashLight _flashLight;
    Material _material;



    void Start()
    {
        _flashLight = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FlashLight>();

        _material = GetComponentInChildren<SpriteRenderer>().material;
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
            _flashLight.lightSources[1].GetComponent<Light2D>().pointLightOuterRadius += increaseAmt;

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
