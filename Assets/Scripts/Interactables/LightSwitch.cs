using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour, IInteractible
{
    public GameObject[] lights;
    public bool isOn;

    public AudioSource audioSource;
    public AudioClip interactClip;
    public AudioClip backgroundClip;

    public event EventHandler OnLightSwitch;


    CanvasGroup _canvasGroup;
    Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        HideCanvas();

        if(isOn)
        {
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(true);
        }
        else if(!isOn)
        {
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(false);
        }
    }

    void Update()
    {
        if(isOn && backgroundClip != null) DOVirtual.Float(audioSource.volume, 1, 1, value => { audioSource.volume = value; });
        else if(!isOn && backgroundClip != null) DOVirtual.Float(audioSource.volume, 0, 1, value => { audioSource.volume = value; });
    }

    public void PlaySound()
    {
        audioSource.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(interactClip);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isOn)
        {
            isOn = true;
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(true);
            PlaySound();
            OnLightSwitch?.Invoke(this, EventArgs.Empty);
        }
        else if(isOn)
        {
            isOn = false;
            for(int i = 0; i < lights.Length; i++) lights[i].SetActive(false);
            PlaySound();
            OnLightSwitch?.Invoke(this, EventArgs.Empty);
        }
    }

    public void HideCanvas()
    {
        _canvasGroup.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        _canvasGroup.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
