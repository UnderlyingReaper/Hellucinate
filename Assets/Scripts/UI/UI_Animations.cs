using DG.Tweening;
using UnityEngine;

public class UI_Animations : MonoBehaviour
{
    public float duration = 0.15f;

    RectTransform _obj;

    Vector2 _orgPos;
    Vector2 _aheadPos;


    void Start()
    {
        _obj = GetComponent<RectTransform>();

        _orgPos = _obj.anchoredPosition;
        _aheadPos = new Vector2(_orgPos.x + 20, _orgPos.y);
    } 

    public void OnHoverEnter_MoveAnimation()
    {
        _obj.DOAnchorPosX(_aheadPos.x, duration).SetUpdate(true);
    }
    public void OnHoverExit_MoveAnimation()
    {
        _obj.DOAnchorPosX(_orgPos.x, duration).SetUpdate(true);
    }
}
