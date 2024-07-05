using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class HiddenLvl_ChoosingOption : MonoBehaviour
{
    public bool allowInteract = true;
    public float range;
    public CanvasGroup canvasGroup;
    public Light2D light1;

    public GameObject targetObj;
    public Transform spawn;
    
    float _distance;
    Transform _player;
    Hellucinate _hellucinate;
    PlayerInputManager _pInputManager;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _hellucinate = _player.GetComponent<Hellucinate>();

        _pInputManager = _player.GetComponent<PlayerInputManager>();
        _pInputManager.playerInput.Player.Interact.performed += Interact;

        canvasGroup.alpha = 0;
        light1.intensity = 0;
        targetObj.SetActive(false);
    }

    void Update()
    {
        if(!allowInteract) return;

        _distance = Vector3.Distance(_player.position, transform.position);

        // Check if player is in range
        if(_distance <= range) StartCoroutine(EnableCanvas());
        else DisableCanvas();
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
        if(_distance > range) return;

        DisableCanvas();
        targetObj.SetActive(true);
        StartCoroutine(_hellucinate.StartHellucinate(3, 2, spawn.position));
    }
}
