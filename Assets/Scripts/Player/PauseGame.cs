using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseGame : MonoBehaviour
{
    public bool isOpen;
    public CanvasGroup pauseMenuCanvas;
    public PauseMenu pauseMenu;
    public SettingsMenu settingsMenu;
    public float duration;

    public RectTransform pauseMenuButtonsHolder;
    public RectTransform pauseText;

    PlayerInputManager _playerInputManager;


    void Start()
    {
        ClosePauseMenu();
        _playerInputManager = GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Pause.performed += Interact;

        pauseMenu.PauseMenuEvent += PauseGameEventHandeler;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isOpen) OpenPauseMenu();
        else if(isOpen)
        {
            ClosePauseMenu();
        }
    }

    public void PauseGameEventHandeler(object sender, PauseMenu.PauseMenuEventArgs e)
    {
        if(e.unPause) ClosePauseMenu();
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;

        pauseMenuButtonsHolder.DOAnchorPos(new Vector2(-180, -100), duration).SetUpdate(true);
        pauseText.DOAnchorPos(new Vector2(0, 75), duration).SetUpdate(true);

        settingsMenu.gameObject.SetActive(false);
        settingsMenu.isOpen = false;

        pauseMenuCanvas.DOFade(0, duration).SetUpdate(true).OnComplete(() => pauseMenuCanvas.gameObject.SetActive(false));

        isOpen = false;
    }

    void OpenPauseMenu()
    {
        pauseMenuCanvas.DOFade(1, duration).SetUpdate(true);

        pauseMenuButtonsHolder.DOAnchorPos(new Vector3(180, -100, 0), duration).SetUpdate(true);
        pauseText.DOAnchorPos(new Vector2(0, -75), duration).SetUpdate(true);

        isOpen = true;
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
    }
}
