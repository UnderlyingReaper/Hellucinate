using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Password_Fuse_Box : MonoBehaviour, IInteractible
{
    [Header("General")]
    public Light2D lightSource;
    public bool isOpen;
    public EventHandler<PuzzleArgs> OnPuzzleComplete;
    public class PuzzleArgs : EventArgs {
        public string puzzleName;
    }

    [Header("Camera")]
    public CanvasGroup fade;
    public GameObject inspectCamera;
    public GameObject gameplayCamera;

    [Header("Puzzles")]
    public GameObject puzzleOne, puzzleTwo;
    public bool puzzle1Complete, puzzle2Complete;
    public Lvl1_FuseCode puzzleOneController;
    public Lvl1_fusesSwitching puzzleTwoController;

    float _distance;
    Transform _player;
    CanvasGroup _canvas;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GetComponentInChildren<CanvasGroup>();

        _canvas.alpha = 0;

        puzzleOneController.OnPuzzle1Complete += OnPuzzle1Complete;
        puzzleTwoController.OnPuzzle2Complete += OnPuzzle2Complete;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isOpen)
        {
            isOpen = true;

            if(!puzzle1Complete) HandlePuzzleOne();
            HandlePuzzleTwo();
        }
        else if(isOpen)
        {
            isOpen = false;

            StartCoroutine(ExitPuzzle());
            StopCoroutine(FocusOnPuzzle(puzzle1Complete? puzzleTwo.transform : puzzleOne.transform ));
        }
    }

    void HandlePuzzleOne()
    {
        StartCoroutine(FocusOnPuzzle(puzzleOne.transform));
        StopCoroutine(ExitPuzzle());
    }
    void HandlePuzzleTwo()
    {
        StartCoroutine(FocusOnPuzzle(puzzleTwo.transform));
        StopCoroutine(ExitPuzzle());
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
    }

    public void OnPuzzle1Complete(object sender, EventArgs e)
    {
        puzzle1Complete = true;
    }
    public void OnPuzzle2Complete(object sender, EventArgs e)
    {
        puzzle2Complete = true;

        DOVirtual.Float(lightSource.intensity, 0.2f, 1, value => { lightSource.intensity = value; });

        OnPuzzleComplete?.Invoke(this, new PuzzleArgs { puzzleName = "fuseSwitchesPuzzle" });
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
