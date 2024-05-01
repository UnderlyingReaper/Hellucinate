using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SetMaxTween : MonoBehaviour
{
    void Start()
    {
        DOTween.SetTweensCapacity(8000,50);
    }
}
