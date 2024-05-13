using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public void TeleportToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
