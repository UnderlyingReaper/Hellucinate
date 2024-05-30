using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lvl1_SecurityCheck_Interact : MonoBehaviour
{
    [Header("General")]
    public Player_Movement player;
    public bool isOpen;
    public float range;
    public EventHandler<SecurityPuzzleCompleteArgs> OnSecurityPuzzleComplete;
    public class SecurityPuzzleCompleteArgs : EventArgs {
        public string puzzleName;
    }

    [Header("Camera")]
    public CanvasGroup fade;
    public GameObject inspectCamera;
    public GameObject gameplayCamera;

    [Header("Puzzles")]
    public bool isPuzzleOneComplete;
    public GameObject puzzleOne;
    public SecurityCheck puzzleOneController;
    public TMP_InputField userInput;
    public CanvasGroup inputFieldCanvasGroup;


    Transform _player;
    PlayerInputManager _pInputmanager;
    CanvasGroup _canvas;
    float _distance;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pInputmanager = _player.GetComponent<PlayerInputManager>();
        _pInputmanager.playerInput.Player.Interact.performed += TryInteract;
        _canvas = GetComponentInChildren<CanvasGroup>();
        _canvas.alpha = 0;

        puzzleOneController.OnPuzzleComplete += OnPuzzleOneSignalReceive;

        inputFieldCanvasGroup.alpha = 0;
    }

    void Update()
    {
        _distance = Vector3.Distance(_player.position, transform.position);

        if(_distance <= range) _canvas.DOFade(1, 1);
        else _canvas.DOFade(0, 1);
    }

    public void TryInteract(InputAction.CallbackContext context)
    {
        if(_distance > range) return;
        if(userInput.isFocused) return;

        if(!isOpen)
        {
            isOpen = true;
            HandlePuzzleOne();
        }
        else if(isOpen)
        {
            isOpen = false;
            HandlePuzzleOne();
        }
    }

    void HandlePuzzleOne()
    {
        if(isOpen)
        {
            StartCoroutine(FocusOnPuzzle(puzzleOne.transform));
            if(!puzzleOneController.isbyPassed) StartCoroutine(puzzleOneController.StartupConsole());
            StopCoroutine(ExitPuzzle());
            inputFieldCanvasGroup.DOFade(1, 2);
            player.allow = false;
        }
        else if(!isOpen)
        {
            StartCoroutine(ExitPuzzle());
            StopCoroutine(FocusOnPuzzle(puzzleOne.transform));
            inputFieldCanvasGroup.DOFade(0, 2);
            player.allow = true;
        }
    }


    IEnumerator FocusOnPuzzle(Transform objectToFocusOn)
    {
        fade.DOFade(1, 1);

        yield return new WaitForSeconds(1);

        inspectCamera.SetActive(true);
        gameplayCamera.SetActive(false);

        inspectCamera.transform.position = objectToFocusOn.position - new Vector3(0, 0, 8);

        fade.DOFade(0, 1);
    }
    IEnumerator ExitPuzzle()
    {
        fade.DOFade(1, 1);

        yield return new WaitForSeconds(1);

        inspectCamera.SetActive(false);
        gameplayCamera.SetActive(true);

        inspectCamera.transform.position = gameplayCamera.transform.position;

        fade.DOFade(0, 1);

        if(isPuzzleOneComplete) OnSecurityPuzzleComplete?.Invoke(this, new SecurityPuzzleCompleteArgs { puzzleName = "SecurityGate" });
    }
    
    public void OnPuzzleOneSignalReceive(object sender, EventArgs e)
    {
        isPuzzleOneComplete = true;
        OnSecurityPuzzleComplete?.Invoke(this, new SecurityPuzzleCompleteArgs { puzzleName = "bypass Security" });
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
