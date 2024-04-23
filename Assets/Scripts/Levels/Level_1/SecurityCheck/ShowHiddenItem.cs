using DG.Tweening;
using UnityEngine;

public class ShowHiddenItem : MonoBehaviour
{
    public Vector3 newPos;
    public float duration = 1;

    void OnMouseOver()
    {
        transform.DOLocalMove(newPos, duration).SetEase(Ease.InOutSine);
    }
}
