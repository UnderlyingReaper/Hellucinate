using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inspect : MonoBehaviour, IInteractible
{
    public bool isOpen = false;
    public GameObject objToInspect;
    public CanvasGroup canvas;

    [Header("Journal Settings")]
    public bool doesAddToJournal = false;
    public bool isAddedToJournal = false;
    public bool isDouble = false;
    public GameObject objPrefab;


    Transform _player;
    Player_Movement _pm;
    Journal_Manager _journal;
    RectTransform _inspectHolder;
    RectTransform _spawnedObj;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pm = _player.GetComponent<Player_Movement>();
        _inspectHolder = GameObject.FindGameObjectWithTag("InspectHolder").GetComponent<RectTransform>();
        if(doesAddToJournal) _journal = GameObject.FindGameObjectWithTag("JournalManager").GetComponent<Journal_Manager>();

        if(canvas != null) canvas.alpha = 0;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!gameObject.activeInHierarchy) return;

        if(!isOpen)
        {
            isOpen = true;
            _pm.allow = false;

            if(doesAddToJournal && !isAddedToJournal)
            {
                _journal.AddPage(objPrefab, isDouble);
                isAddedToJournal = true;
            }

            _spawnedObj = Instantiate(objToInspect, _inspectHolder).GetComponent<RectTransform>();
            _inspectHolder.GetComponent<CanvasGroup>().DOFade(1, 1).SetUpdate(true);
            _spawnedObj.anchoredPosition = new Vector2(0, 0);

            Time.timeScale = 0;
        }
        else if(isOpen)
        {
            Time.timeScale = 1;
            _pm.allow = true;
            isOpen = false;
            Destroy(_spawnedObj.gameObject, 1.1f);
            _inspectHolder.GetComponent<CanvasGroup>().DOFade(0, 1).SetUpdate(true);
        }
    }

    public void HideCanvas()
    {
        if(canvas != null) canvas.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        if(canvas != null) canvas.DOFade(1, 1);
    }

    public void OnInteractKeyUp() {}

    public void OnInteractKeyDown() {}
}
