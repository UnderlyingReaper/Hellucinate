using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;

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
