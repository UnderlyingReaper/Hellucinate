using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Lvl1_SecurityCheck_Interact : MonoBehaviour
{
    [Header("General")]
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


    Transform _player;
    CanvasGroup _canvas;
    int count = 0;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GetComponentInChildren<CanvasGroup>();
        _canvas.alpha = 0;

        puzzleOneController.OnPuzzleComplete += OnPuzzleOneSignalReceive;
    }

    void Update()
    {
        float distance = Vector3.Distance(_player.position, transform.position);

        if(distance <= range)
        {
            _canvas.DOFade(1, 1);

            if(Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                isOpen = true;
                count++;

                HandlePuzzleOne();
            }
            else if(Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                isOpen = false;

                HandlePuzzleOne();
            }
        }
        else
        {
            _canvas.DOFade(0, 1);
        }
    }

    void HandlePuzzleOne()
    {
        if(isOpen)
        {
            StartCoroutine(FocusOnPuzzle(puzzleOne.transform));
            if(count == 1) StartCoroutine(puzzleOneController.StartupConsole());
            else StartCoroutine(puzzleOneController.ScanHand());
            StopCoroutine(ExitPuzzle());
        }
        else if(!isOpen)
        {
            StartCoroutine(ExitPuzzle());
            StopCoroutine(FocusOnPuzzle(puzzleOne.transform));
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
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
