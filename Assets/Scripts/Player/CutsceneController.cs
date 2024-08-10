using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    public CanvasGroup cutSceneFade;
    public Transform cutSceneCam;

    public float fadeDuration;




    public IEnumerator PlayCutscene(Vector3 pos, float delay = 2.5f)
    {
        cutSceneFade.DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        cutSceneCam.gameObject.SetActive(true);
        cutSceneCam.position = new Vector3(pos.x, pos.y, cutSceneCam.position.z);
        cutSceneFade.DOFade(0, fadeDuration);

        yield return new WaitForSeconds(delay + fadeDuration);

        cutSceneFade.DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        cutSceneCam.gameObject.SetActive(false);
        cutSceneFade.DOFade(0, fadeDuration);
    }
}
