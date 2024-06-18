using DG.Tweening;
using UnityEngine;

public class TutorialTexts : MonoBehaviour
{
    public float range;
    public CanvasGroup canvasGroup;

    Transform _player;
    float _distance;



    void Update()
    {
        _distance = Vector2.Distance(_player.transform.position, transform.position);

        if(_distance <= range)
        {
            canvasGroup.DOFade(1, 0.3f);
        }
        else
        {
            canvasGroup.DOFade(0, 0.3f);
        }
    }
}
