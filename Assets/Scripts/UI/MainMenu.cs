using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public CanvasGroup transitionCg;
    public RectTransform loadingBar;

    public AudioSource audioSource;
    public AudioClip gameStartClip;


    void Start()
    {
        transitionCg.DOFade(0, 2).SetUpdate(true);
        loadingBar.DORotate(new Vector3(0, 0, -360), 2, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    public void OpenSettings()
    {
        settingsMenu.gameObject.SetActive(true);
        settingsMenu.isOpen = true;
        gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator StartGame()
    {   
        audioSource.PlayOneShot(gameStartClip);

        transitionCg.DOFade(1, 2);
        yield return new WaitForSeconds(2);
        loadingBar.GetComponent<CanvasGroup>().DOFade(1, 1);

        yield return new WaitForSeconds(2);

        loadingBar.GetComponent<CanvasGroup>().DOFade(0, 1);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Level_1");
    }
}
