using DG.Tweening;
using UnityEngine;

public class SetMaxTween : MonoBehaviour
{
    void Start()
    {
        DOTween.SetTweensCapacity(50000,50);
    }
}
