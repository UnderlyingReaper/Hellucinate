using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Engine : MonoBehaviour, IInteractible
{
    public bool isRunning;
    public bool isAttached;
    public bool isInteractible;
    public string itemKey;
    public CanvasGroup canvas;
    public Transform cogWheel;

    public Light2D lightSource;
    public PlayerText_Trigger playerTextTrigger;

    public string text;
    public string text2;
    public float displayTime;


    public Action OnStartRunning;


    Inventory_System _invSystem;
    PlayerTextDisplay _playerTextDisplay;



    void Start()
    {
        _invSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        _playerTextDisplay = _invSystem.GetComponent<PlayerTextDisplay>();

        canvas.alpha = 0;
        if(playerTextTrigger != null) playerTextTrigger.enabled = false;
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
            cogWheel.DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
            isRunning = true;
            isInteractible = false;
            canvas.DOFade(0, 1);
            lightSource.color = Color.green;

            OnStartRunning?.Invoke();

            if(playerTextTrigger != null) playerTextTrigger.enabled = true;
            return;
        }

        bool isInInv = _invSystem.CheckForItem(itemKey);
        if (isInInv)
        {
            _invSystem.RemoveItem(itemKey);
            cogWheel.gameObject.SetActive(true);
            isAttached = true;
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(text2, displayTime));
        }
        else
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(text, displayTime));
        }
    }

    public void OnInteractKeyDown(){}

    public void OnInteractKeyUp(){}

    public void ShowCanvas()
    {
        if(isInteractible) canvas.DOFade(1, 1);
    }
}
