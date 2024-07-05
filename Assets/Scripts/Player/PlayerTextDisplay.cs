using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public CanvasGroup playerTextFade;

    public string startingTxt;
    public float delay;


    void Start()
    {
        StartCoroutine(DisplayPlayerText(startingTxt, delay, 15f));
    }

    public IEnumerator DisplayPlayerText(string txt, float displayTime, float initialDelay = 0)
    {
        yield return new WaitForSeconds(initialDelay);

        playerText.text = txt;

        playerTextFade.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.3f + displayTime);

        playerTextFade.DOFade(0, 0.5f);
    }
}
