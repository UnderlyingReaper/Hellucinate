using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ElectricBox : MonoBehaviour
{
    [Header("General")]
    public bool isOpen;
    public float range;
    public EventHandler<ElectricBoxPuzzleCompleteArgs> OnElectricBoxPuzzleComplete;
    public class ElectricBoxPuzzleCompleteArgs : EventArgs {
        public string puzzleName;
    }

    [Header("Camera")]
    public CanvasGroup fade;
    public GameObject inspectCamera;
    public GameObject gameplayCamera;

    [Header("Puzzles")]
    public bool isPuzzleOneComplete;
    public GameObject puzzleOne;
    public PuzzleOneController puzzleOneController;


    Transform _player;
    CanvasGroup _canvas;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GetComponentInChildren<CanvasGroup>();

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
        bool doesPlayerHaveWires = _player.GetComponent<Inventory_System>().CheckForItem("Wires");

        if(doesPlayerHaveWires)
        {
            _player.GetComponent<Inventory_System>().RemoveItem("Wires");
            puzzleOneController.enabled = true;
        }

        if(isOpen)
        {
            StartCoroutine(FocusOnPuzzle(puzzleOne.transform));
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

        if(isPuzzleOneComplete) OnElectricBoxPuzzleComplete?.Invoke(this, new ElectricBoxPuzzleCompleteArgs { puzzleName = "ElectricBox" });
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
