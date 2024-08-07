using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipe_Interactible : MonoBehaviour, IInteractible
{
    public CanvasGroup canvas;
    public bool isInteractible;
    public bool isOpen;
    public event EventHandler<PipeStateInfo> OnPipeStateChange;
    public class PipeStateInfo : EventArgs {
        public bool isOpen;
    }



    void Start()
    {
        canvas.alpha = 0;
    }


    public void HideCanvas()
    {
        if(isInteractible) canvas.DOFade(0, 1);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isInteractible) return;

        if(isOpen)
        {
            isOpen = false;
        }
        else
        {
            isOpen = true;
        }

        OnPipeStateChange?.Invoke(this, new PipeStateInfo { isOpen = isOpen });
    }

    public void OnInteractKeyDown() {}

    public void OnInteractKeyUp() {}

    public void ShowCanvas()
    {
        if(isInteractible) canvas.DOFade(1, 1);
    }
}