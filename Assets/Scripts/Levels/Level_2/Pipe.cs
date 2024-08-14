using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipe : MonoBehaviour, IInteractible
{
    public CanvasGroup canvas;
    public ParticleSystem oilLeakVfx;
    public Engine engine;
    public AudioClip turnValve;

    public bool isInteractible;
    public bool isAttached;
    public bool isOpen;
    public string itemKey;
    public string playerText;
    public float textDuration;

    public Action OnValveOpen;
    public Action OnValveAttached;


    SpriteRenderer _spriteRenderer;
    Inventory_System _invSystem;
    PlayerTextDisplay _playerTextDisplay;
    AudioSource _source;



    void Start()
    {
        _invSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        _playerTextDisplay = _invSystem.GetComponent<PlayerTextDisplay>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _source = GetComponent<AudioSource>();

        canvas.alpha = 0;
        _spriteRenderer.enabled = false;
        engine.OnStartRunning += OnEngineStart;
    }

    void OnEngineStart()
    {
        if(!isAttached) oilLeakVfx.Play();
    }


    public void HideCanvas()
    {
        if(isInteractible) canvas.DOFade(0, 1);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isInteractible) return;

        if(isAttached)
        {
            isOpen = true;
            OnValveOpen?.Invoke();
            isInteractible = false;
            canvas.DOFade(0, 1);
            _source.PlayOneShot(turnValve);
            return;
        }

        bool doesHaveItem = _invSystem.CheckForItem(itemKey);
        if(doesHaveItem)
        {
            _spriteRenderer.enabled = true;
            isAttached = true;
            oilLeakVfx.Stop();
            OnValveAttached?.Invoke();
            return;
        }
        else
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(playerText, textDuration));
        }
    }

    public void OnInteractKeyDown() {}

    public void OnInteractKeyUp() {}

    public void ShowCanvas()
    {
        if(isInteractible) canvas.DOFade(1, 1);
    }
}
