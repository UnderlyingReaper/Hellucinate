using DG.Tweening;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    public bool isOpen = false;
    public float range = 1;
    public GameObject objToInspect;
    public CanvasGroup canvas;


    Transform _player;
    RectTransform _inspectHolder;
    RectTransform _spawnedObj;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _inspectHolder = GameObject.FindGameObjectWithTag("InspectHolder").GetComponent<RectTransform>();

        if(canvas != null) canvas.alpha = 0;
    }

    void Update()
    {
        float distance = Vector2.Distance(_player.position, transform.position);

        if(distance <= range)
        {
            if(canvas != null) canvas.DOFade(1, 1);

            if(Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                isOpen = true;
                _spawnedObj = Instantiate(objToInspect, _inspectHolder).GetComponent<RectTransform>();
                _inspectHolder.GetComponent<CanvasGroup>().DOFade(1, 1).SetUpdate(true);
                _spawnedObj.anchoredPosition = new Vector2(0, 0);
                Time.timeScale = 0;
            }
            else if(Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                Time.timeScale = 1;
                isOpen = false;
                Destroy(_spawnedObj.gameObject, 1.1f);
                _inspectHolder.GetComponent<CanvasGroup>().DOFade(0, 1).SetUpdate(true);
            }
        }
        else
        {
            if(canvas != null) canvas.DOFade(0, 1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
