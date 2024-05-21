using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public CanvasGroup transitionCg;

    public event EventHandler<PauseMenuEventArgs> PauseMenuEvent;

    public class PauseMenuEventArgs : EventArgs {
        public bool unPause = false;
    }

    public void ResumeGame()
    {
        PauseMenuEvent?.Invoke(this, new PauseMenuEventArgs { unPause = true });
    }

    public void OpenMainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(MainMenu());
        transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    IEnumerator MainMenu()
    {
        transitionCg.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        Debug.Log("Trans");
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.gameObject.SetActive(true);
        settingsMenu.isOpen = true;
        gameObject.SetActive(false);
    }
}
