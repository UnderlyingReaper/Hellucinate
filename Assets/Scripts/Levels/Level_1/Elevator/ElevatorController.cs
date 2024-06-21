using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorController : MonoBehaviour, IInteractible
{
    [Header("General")]
    public bool allowInteraction;
    public Handle_Barricade barricade;
    public CanvasGroup fade;
    public AudioSource audioSource;
    public ElectricBox electricBox;

    [Header("Elevator")]
    public Transform doorR;
    public Transform doorL;
    public Transform teleportLocation;
    public Camera_Follow camera_Follow;
    public Animator animController;


    CanvasGroup _canvas;
    Transform _player;

    bool _isPowerOn, _isBarricadeRemoved = false;



    void Start()
    {
        _canvas = GetComponentInChildren<CanvasGroup>();
        _canvas.alpha = 0;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        allowInteraction = false;

        electricBox.OnElectricBoxPuzzleComplete += OnWiresConnected;
        barricade.OnBarricadesRemoved += OnBarricadesRemoved;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!allowInteraction) return;

        StartCoroutine(TeleportPlayer());
    }

    public void OnWiresConnected(object sender, ElectricBox.ElectricBoxPuzzleCompleteArgs e)
    {
        _isPowerOn = true;
        CheckForRequirements();
    }
    public void OnBarricadesRemoved(object sender, EventArgs e)
    {
        _isBarricadeRemoved = true;
        CheckForRequirements();
    }
    void CheckForRequirements()
    {
        if(_isBarricadeRemoved && _isPowerOn) allowInteraction = true;
    }


    IEnumerator TeleportPlayer()
    {
        fade.DOFade(1, 2);
        _player.GetComponent<Player_Movement>().allow = false;

        yield return new WaitForSeconds(2);
        _player.position = teleportLocation.position;
        _player.SetParent(teleportLocation);
        camera_Follow.smoothTime = 0.08f;
        camera_Follow.offset.y = 0.5f;
        yield return new WaitForSeconds(2);

        _player.GetComponent<Player_Movement>().allow = true;
        fade.DOFade(0, 2);
        animController.SetTrigger("Start");
    }

    void OpenDoor()
    {
        doorL.DOScaleX(0, 2f).SetEase(Ease.Linear);
        doorR.DOScaleX(0, 2f).SetEase(Ease.Linear);
    }
    void CloseDoor()
    {
        doorL.DOScaleX(1, 2f).SetEase(Ease.Linear);
        doorR.DOScaleX(1, 2f).SetEase(Ease.Linear);
    }

    public void HideCanvas()
    {
        if(!allowInteraction) return;
        _canvas.DOFade(0, 1);
        CloseDoor();
    }

    public void ShowCanvas()
    {
        if(!allowInteraction) return;
        _canvas.DOFade(1, 1);
        OpenDoor();
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
