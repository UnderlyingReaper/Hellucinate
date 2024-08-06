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


    public Action OnStartRunning;


    Inventory_System _invSystem;



    void Start()
    {
        _invSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
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
        }
        else if(_invSystem.CheckForItem(itemKey))
        {
            _invSystem.RemoveItem(itemKey);
            cogWheel.gameObject.SetActive(true);
            isAttached = true;
        }
    }

    public void OnInteractKeyDown(){}

    public void OnInteractKeyUp(){}

    public void ShowCanvas()
    {
        if(isInteractible) canvas.DOFade(1, 1);
    }
}
