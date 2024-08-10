using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerTextDisplay : MonoBehaviour
{
    public TextMeshProUGUI playerText;
    public CanvasGroup playerTextFade;
    public CanvasGroup userInputFade;


    public float initialDelay = 15;
    public string startingTxt;
    public float delay;


    void Start()
    {
        StartCoroutine(DisplayPlayerText(startingTxt, delay, initialDelay));
    }

    public IEnumerator DisplayPlayerText(string txt, float displayTime, float initialDelay = 0)
    {
        yield return new WaitForSeconds(initialDelay);

        Debug.Log("Initialized Text");
        playerText.text = txt;

        userInputFade.DOFade(0, 0.5f);
        playerTextFade.DOFade(1, 0.5f);

        yield return new WaitForSeconds(displayTime);

        Debug.Log("Hiding Text");
        userInputFade.DOFade(1, 0.5f);
        playerTextFade.DOFade(0, 0.5f);
    }
}
