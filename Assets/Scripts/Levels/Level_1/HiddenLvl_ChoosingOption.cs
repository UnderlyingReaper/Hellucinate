using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class HiddenLvl_ChoosingOption : MonoBehaviour, IInteractible
{
    public bool allowInteract = true;
    public CanvasGroup canvasGroup;
    public Light2D light1;

    public GameObject targetObj;
    public Transform spawn;
    
    Transform _player;
    Hellucinate _hellucinate;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _hellucinate = _player.GetComponent<Hellucinate>();

        canvasGroup.alpha = 0;
        light1.intensity = 0;
        if(targetObj != null) targetObj.SetActive(false);
    }

    IEnumerator EnableCanvas()
    {
        DOVirtual.Float(light1.intensity, 0.2f, 1f, value => { light1.intensity = value; });
        yield return new WaitForSeconds(1f);
        canvasGroup.DOFade(1, 2f);
    }
    void DisableCanvas()
    {
        DOVirtual.Float(light1.intensity, 0, 2f, value => { light1.intensity = value; });
        canvasGroup.DOFade(0, 2f);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!gameObject.activeSelf) return;
        if(!allowInteract) return;

        allowInteract = false;

        DisableCanvas();
        if(targetObj != null) targetObj.SetActive(true);
        StartCoroutine(_hellucinate.StartHellucinate(3, 2, spawn.position));
        
    }

    public void HideCanvas()
    {
        if(allowInteract) DisableCanvas();
    }

    public void ShowCanvas()
    {
        if(allowInteract) StartCoroutine(EnableCanvas());
    }

    public void OnInteractKeyUp()
    {
        
    }

    public void OnInteractKeyDown()
    {
        
    }
}
