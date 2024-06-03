using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Hellucinate : MonoBehaviour
{
    public Camera_Follow cameraHolder;
    public CanvasGroup fade;
    public Volume volume;


    DepthOfField _dof;
    Sanity _sanity;
    Rigidbody2D _rb;



    void Start()
    {
        _sanity = GetComponent<Sanity>();
        _rb = GetComponent<Rigidbody2D>();

        volume.profile.TryGet(out _dof);
    }

    public IEnumerator StartHellucinate(int numberOfBlinks, int blinkNoToTeleport, Vector3 desiredPosition)
    {
        _sanity.pauseSanity = true;
        DOVirtual.Float(_rb.drag, 50, 2, value => { _rb.drag = value; });
        DOVirtual.Float(_dof.focusDistance.value, 1.5f, 2, value => { _dof.focusDistance.value = value; });

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < numberOfBlinks; i++)
        {
            fade.DOFade(1, 0.8f);
            yield return new WaitForSeconds(0.8f);

            if(i == blinkNoToTeleport)
            {
                transform.position = desiredPosition;
                cameraHolder.transform.position = desiredPosition + cameraHolder.offset;
            }

            fade.DOFade(0, 0.8f);
            yield return new WaitForSeconds(1.2f);
        }

        DOVirtual.Float(_rb.drag, 0, 2, value => { _rb.drag = value; });
        DOVirtual.Float(_dof.focusDistance.value, 5f, 2, value => { _dof.focusDistance.value = value; }).OnComplete(() => _sanity.pauseSanity = false);
    }
}
