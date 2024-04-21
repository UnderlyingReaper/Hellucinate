using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Password_Fuse_Box : MonoBehaviour
{
    [Header("General")]
    public bool isOpen;
    public float range;
    public EventHandler<PasswordFuseBoxArgs> OnPuzzleComplete;
    public class PasswordFuseBoxArgs : EventArgs {
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
    public Lvl1_FuseSwitching puzzleTwoController;

    Transform _player;
    CanvasGroup _canvas;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GetComponentInChildren<CanvasGroup>();

        _canvas.alpha = 0;

        puzzleOneController.OnPuzzle1Complete += OnPuzzle1Complete;
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

                if(!puzzle1Complete) HandlePuzzleOne();
            }
            else if(Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                isOpen = false;

                StartCoroutine(ExitPuzzle());
                StopCoroutine(FocusOnPuzzle(puzzle1Complete? puzzleTwo.transform : puzzleOne.transform ));
            }
        }
        else
        {
            _canvas.DOFade(0, 1);
        }
    }

    void HandlePuzzleOne()
    {
        StartCoroutine(FocusOnPuzzle(puzzleOne.transform));
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
