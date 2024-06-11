using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inspect : MonoBehaviour
{
    public bool isOpen = false;
    public float range = 1;
    public GameObject objToInspect;
    public CanvasGroup canvas;

    float _distance;
    Transform _player;
    Player_Movement _pm;
    PlayerInputManager _pInputManager;
    RectTransform _inspectHolder;
    RectTransform _spawnedObj;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pm = _player.GetComponent<Player_Movement>();
        _pInputManager = _player.GetComponent<PlayerInputManager>();
        _pInputManager.playerInput.Player.Interact.performed += TryInspect;

        _inspectHolder = GameObject.FindGameObjectWithTag("InspectHolder").GetComponent<RectTransform>();

        if(canvas != null) canvas.alpha = 0;
    }

    void Update()
    {
        _distance = Vector2.Distance(_player.position, transform.position);

        if(canvas == null) return;
        if(_distance <= range) canvas.DOFade(1, 1);
        else canvas.DOFade(0, 1);
    }

    public void TryInspect(InputAction.CallbackContext context)
    {
        if(!gameObject.activeInHierarchy) return;
        if(_distance > range) return;

        if(!isOpen)
        {
            isOpen = true;
            _pm.allow = false;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
