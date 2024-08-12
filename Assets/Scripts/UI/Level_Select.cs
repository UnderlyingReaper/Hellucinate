using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Select : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip gameStartClip;
    public CanvasGroup transitionCg;
    public RectTransform loadingBar;
    public MainMenu mainMenu;


    Vector3 _orgScale;

    void Start()
    {
        transitionCg.DOFade(0, 2).SetUpdate(true);
        loadingBar.DORotate(new Vector3(0, 0, -360), 2, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void OnButtonEnter(RectTransform obj)
    {
        _orgScale = obj.localScale;
        obj.DOScale(obj.localScale * 1.1f, 0.15f);
    }

    public void OnButtonDown(string levelName)
    {
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(StartGame(levelName));
    }

    public void OnButtonExit(RectTransform obj)
    {
        obj.DOScale(_orgScale, 0.15f);
    }

    public void BackButton()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public IEnumerator StartGame(string levelName)
    {   
        audioSource.PlayOneShot(gameStartClip);

        transitionCg.DOFade(1, 2);
        yield return new WaitForSeconds(2);
        loadingBar.GetComponent<CanvasGroup>().DOFade(1, 1);

        yield return new WaitForSeconds(2);

        loadingBar.GetComponent<CanvasGroup>().DOFade(0, 1);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelName);
    }
}
