using DG.Tweening;
using UnityEngine;

public class ElectricBox : MonoBehaviour
{
    [Header("General")]
    public bool isOpen;
    public float range;

    public bool isPuzzleOneComplete;
    public RectTransform puzzleOne;


    Transform _player;
    CanvasGroup _canvas;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _canvas = GetComponentInChildren<CanvasGroup>();
        
        puzzleOne.anchoredPosition = new Vector2(0, -1000);
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

                if(!isPuzzleOneComplete) puzzleOne.DOAnchorPos(new Vector2(0, 0), 2).SetEase(Ease.InOutSine);
            }
            else if(Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                isOpen = false;

                puzzleOne.DOAnchorPos(new Vector2(0, -1000), 2).SetEase(Ease.InOutSine);
            }
        }
        else
        {
            _canvas.DOFade(0, 1);
        }
    }




    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
