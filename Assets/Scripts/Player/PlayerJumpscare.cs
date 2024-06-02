using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerJumpscare : MonoBehaviour
{
    public Animator animator;
    public Sanity sanity;
    public AudioSource source;
    public Transform cam;

    public float _shakeAmplitude;
    float _currAmplitude; 

    public void PlayJumpScare(AudioClip screamAudio, float sanitLossAmt)
    {
        animator.SetTrigger("PlayJumpScare");
        source.PlayOneShot(screamAudio);
        StartCoroutine(CameraAnimation());

        sanity.sanity -= sanitLossAmt;
    }

    IEnumerator CameraAnimation()
    {
        cam.DOLocalMoveZ(-1, 0.2f);

        yield return new WaitForSeconds(0.2f);

        cam.DOLocalMoveZ(2, 0.2f);
        cam.DOLocalRotate(new Vector3(0, 0, 5), 0.5f);

        yield return new WaitForSeconds(0.8f);

        cam.DOLocalMoveZ(0, 0.5f);
        cam.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
    }

    public void StartEndShake(int option)
    {
        if(option == 1) DOVirtual.Float(_currAmplitude, _shakeAmplitude, 0.1f, value => { _currAmplitude = value; });
        else if(option == 0) DOVirtual.Float(_currAmplitude, 0, 0.1f, value => { _currAmplitude = value; });
    }
}
