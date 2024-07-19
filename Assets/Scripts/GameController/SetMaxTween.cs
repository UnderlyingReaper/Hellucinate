using DG.Tweening;
using UnityEngine;

public class SetMaxTween : MonoBehaviour
{
    void Start()
    {
        DOTween.SetTweensCapacity(100000,50);
    }
}
