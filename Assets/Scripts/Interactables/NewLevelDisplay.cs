using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NewLevelDisplay : MonoBehaviour
{
    public string lvlName;
    public TextMeshProUGUI lvlDisplay;
    public float fadeDuration = 2;



    void Start()
    {
        lvlDisplay.GetComponent<CanvasGroup>().alpha = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            StartCoroutine(ShowLevel());
        }
    }

    IEnumerator ShowLevel()
    {
        lvlDisplay.text = lvlName;
        lvlDisplay.GetComponent<CanvasGroup>().DOFade(1, fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 4);

        lvlDisplay.GetComponent<CanvasGroup>().DOFade(0, fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 0.01f);

        Destroy(gameObject);
    }
}
