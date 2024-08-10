using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piston : MonoBehaviour, IInteractible
{
    public CanvasGroup canvas;
    public ParticleSystem steamVfx, oilLeakVfx;
    public GameObject pumpHolder;
    public Transform cutScenePos;

    public bool isInteractible;
    public bool isAttached;
    public bool isRunning;

    public Action OnPistonStart;
    public string itemKey;

    [Header("Additions Params")]
    public Engine engine;
    public Pipe pipe;
    public Pipe_Interactible pipe_Interactible;
    public bool isEngineRunning;
    public bool isPump1Connetced;
    public bool isPump2Connected;

    [Header("Player Text")]
    public string textDisplay;
    public float displayTime;
    public string engineTextDisplay;
    public float engineDisplayTime;
    public string pipeTextDisplay;
    public float pipeDisplayTime;
    public string pipe2TextDisplay;
    public float pipe2DisplayTime;


    Inventory_System _invSystem;
    CutsceneController _cutsceneController;
    PlayerTextDisplay _playerTextDisplay;
    Animator _animator;

    void Start()
    {
        _invSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory_System>();
        _playerTextDisplay = _invSystem.GetComponent<PlayerTextDisplay>();
        _cutsceneController = _invSystem.GetComponent<CutsceneController>();

        _animator = GetComponent<Animator>();

        engine.OnStartRunning += EngineStart;
        pipe.OnValveOpen += PipeStart;
        pipe_Interactible.OnPipeStateChange += Pipe2Start;
    }

    void EngineStart() => isEngineRunning = true;
    void PipeStart() => isPump1Connetced = true;
    void Pipe2Start(object sender, Pipe_Interactible.PipeStateInfo e)
    {
        if(e.isOpen && isEngineRunning && !isAttached)
        {
            oilLeakVfx.gameObject.SetActive(true);
            return;
        }
        else oilLeakVfx.gameObject.SetActive(false);

        isPump2Connected = e.isOpen;
    }

    public void HideCanvas()
    {
        if(isInteractible) canvas.DOFade(0, 1);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isInteractible) return;

        if(!isAttached)
        {
            bool checkItem = _invSystem.CheckForItem(itemKey);
            if(checkItem)
            {
                isAttached = true;
                pumpHolder.SetActive(true);
            }
            else
            {
                StartCoroutine(_playerTextDisplay.DisplayPlayerText(textDisplay, displayTime));
            }

            return;
        }

        if(!isEngineRunning)
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(engineTextDisplay, engineDisplayTime));
            return;
        }
        if(!isPump1Connetced)
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(pipeTextDisplay, pipeDisplayTime));
            return;
        }
        if(!isPump2Connected)
        {
            StartCoroutine(_playerTextDisplay.DisplayPlayerText(pipe2TextDisplay, pipe2DisplayTime));
            return;
        }

        if(isAttached)
        {
            _animator.SetTrigger("Start");
            steamVfx.gameObject.SetActive(true);
            isRunning = true;
            pipe_Interactible.isInteractible = false;
            isInteractible = false;
            canvas.DOFade(0, 1);

            StartCoroutine(_cutsceneController.PlayCutscene(cutScenePos.position));

            OnPistonStart?.Invoke();
            return;
        }
    }

    public void OnInteractKeyDown() {}

    public void OnInteractKeyUp() {}

    public void ShowCanvas()
    {
        if(isInteractible) canvas.DOFade(1, 1);
    }
}
