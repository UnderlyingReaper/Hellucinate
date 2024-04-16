using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Light_Sway : MonoBehaviour
{
    public float swayDuration = 2;
    public float maxSwayAngle = 10;
    public Ease easeMode = Ease.InOutSine;


    void Start()
    {   
        StartCoroutine(LightSway());
    }
    IEnumerator LightSway()
    {
        while(true)
        {
            transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(180, 0, maxSwayAngle)), swayDuration).SetEase(easeMode);

            yield return new WaitForSeconds(swayDuration);

            transform.DOLocalRotateQuaternion(Quaternion.Euler(new Vector3(180, 0, -maxSwayAngle)), swayDuration).SetEase(easeMode);

            yield return new WaitForSeconds(swayDuration);
        }
    }
}
