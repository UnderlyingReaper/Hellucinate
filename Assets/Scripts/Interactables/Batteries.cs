using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Batteries : MonoBehaviour, IInteractible
{
    public bool allowInteraction = true;
    public FlashLight flashLight;
    public float batteryAmt = 1;
    public AudioSource source;
    public CanvasGroup canvas;


    Transform _player;
    Material _material;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _material = GetComponent<SpriteRenderer>().material;

        if(canvas != null) canvas.alpha = 0;
    }

    public void PickupBattery()
    {
        allowInteraction = false;
        HideCanvas();
        flashLight.currBatteries += batteryAmt;
        PlayPickupSound(); // play sound
        
        // Start desolving the shader
        if(_material != null) DOVirtual.Float(_material.GetFloat("_Fade"), 0, 2, value => {_material.SetFloat("_Fade", value); });

        // Hide canvas
        canvas.DOFade(0, 1f);
        
        // Destroy the item
        Destroy(gameObject, 2.1f);
    }

    public void PlayPickupSound()
    {
        source.volume = Random.Range(0.8f, 1.2f);
        source.pitch = Random.Range(0.8f, 1.2f);

        source.PlayOneShot(source.clip);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(allowInteraction) PickupBattery();
    }

    public void HideCanvas()
    {
        if(allowInteraction) canvas.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        if(allowInteraction) canvas.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
