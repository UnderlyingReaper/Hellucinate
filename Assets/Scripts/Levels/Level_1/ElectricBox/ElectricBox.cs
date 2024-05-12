using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

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


    float _distance;
    Transform _player;
    PlayerInputManager _pInputManager;
    CanvasGroup _canvas;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pInputManager = _player.GetComponent<PlayerInputManager>();
        _pInputManager.playerInput.Player.Interact.performed += TryInteract;
        _canvas = GetComponentInChildren<CanvasGroup>();
        _canvas.alpha = 0;

        puzzleOneController.OnPuzzleComplete += OnPuzzleOneSignalReceive;
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

        if(!isOpen)
        {
            isOpen = true;
            _player.GetComponent<Player_Movement>().allow = false;
            HandlePuzzleOne();
        }
        else if(isOpen)
        {
            isOpen = false;
            _player.GetComponent<Player_Movement>().allow = true;
            HandlePuzzleOne();
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
