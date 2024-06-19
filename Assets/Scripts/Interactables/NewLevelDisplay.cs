using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NewLevelDisplay : MonoBehaviour
{
    public string lvlName;
    public TextMeshProUGUI lvlDisplay;
    public float fadeDuration = 2;
    bool _allow = true;



    void Start()
    {
        lvlDisplay.GetComponent<CanvasGroup>().alpha = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && _allow)
        {
            StartCoroutine(ShowLevel());
            _allow = false;
        }
    }

    IEnumerator ShowLevel()
    {
        lvlDisplay.text = lvlName;
        lvlDisplay.GetComponent<CanvasGroup>().DOFade(1, fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 4);

        lvlDisplay.GetComponent<CanvasGroup>().DOFade(0, fadeDuration);

        Destroy(gameObject, 0.1f);
    }
}
