using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lvl1_SecurityCheck_Interact : MonoBehaviour, IInteractible
{
    [Header("General")]
    public Player_Movement player;
    public bool isOpen;
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


    CanvasGroup _canvas;


    void Start()
    {
        _canvas = GetComponentInChildren<CanvasGroup>();
        _canvas.alpha = 0;

        puzzleOneController.OnPuzzleComplete += OnPuzzleOneSignalReceive;

        inputFieldCanvasGroup.alpha = 0;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(userInput.isFocused) return;

        if(!isOpen)
        {
            isOpen = true;
            player.allow = false;
            HandlePuzzleOne();
        }
        else if(isOpen)
        {
            isOpen = false;
            player.allow = true;
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
            
        }
        else if(!isOpen)
        {
            StartCoroutine(ExitPuzzle());
            StopCoroutine(FocusOnPuzzle(puzzleOne.transform));
            inputFieldCanvasGroup.DOFade(0, 2);
            
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

    public void HideCanvas()
    {
        _canvas.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        _canvas.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
