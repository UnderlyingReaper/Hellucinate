using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public Level_Select level_Select;

    public void PlayGame()
    {
        level_Select.gameObject.SetActive(true);
        gameObject.SetActive(false);
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

    
}
