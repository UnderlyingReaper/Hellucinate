using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public GameObject previousMenu;
    public bool isOpen;

    [Header("Visuals")]
    public Volume volume;
    public TMP_Dropdown displayModeUI;
    public TMP_Dropdown resolutionUI;
    public TMP_Dropdown bloomUI;
    public TMP_Dropdown filmGrainUI;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider MasterSlider;
    public Slider SfxSlider;
    public Slider musicSlider;
    public Slider ambienceSlider;
    

    Bloom _bloom;
    FilmGrain _filmGrain;
    FullScreenMode _fullscreenMode;
    Resolution[] _resolutions;

    void Start()
    {
        _resolutions = Screen.resolutions;
        List<String> resOptions = new List<string>();
        int currResIndex = 0;
        for(int i = 0; i < _resolutions.Length; i++)
        {
            resOptions.Add(_resolutions[i].width + " x " + _resolutions[i].height);

            if(_resolutions[i].width == Screen.currentResolution.width &&
               _resolutions[i].height == Screen.currentResolution.height) currResIndex = i;
        }

        resolutionUI.ClearOptions();
        resolutionUI.AddOptions(resOptions);
        resolutionUI.value = currResIndex;
        resolutionUI.RefreshShownValue();

        // ACTUAL START FUNCTION

        volume.profile.TryGet(out _bloom);
        volume.profile.TryGet(out _filmGrain);

        GetSettings();

        displayModeUI.onValueChanged.AddListener(OnDisplayModeChange);
        resolutionUI.onValueChanged.AddListener(OnResolutionChange);
        bloomUI.onValueChanged.AddListener(OnBloomChange);
        filmGrainUI.onValueChanged.AddListener(OnFilmGrainChange);

        MasterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        SfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        ambienceSlider.onValueChanged.AddListener(ChangeAmbienceVolume);

        gameObject.SetActive(false);
        isOpen = false;
    }

    void GetSettings()
    {   
        #region Access Displaymode
        int displayModeVal = PlayerPrefs.GetInt("DisplayMode");
        if(displayModeVal == 0) _fullscreenMode = FullScreenMode.FullScreenWindow;
        else if(displayModeVal == 1) _fullscreenMode = FullScreenMode.ExclusiveFullScreen;
        else if(displayModeVal == 2) _fullscreenMode = FullScreenMode.Windowed;
        else if(displayModeVal == 3) _fullscreenMode = FullScreenMode.MaximizedWindow;
        displayModeUI.value = displayModeVal;
        Screen.fullScreenMode = _fullscreenMode;
        #endregion

        #region Acces bloom
        int bloomStoredVal = PlayerPrefs.GetInt("Bloom");
        if(bloomStoredVal == 0) _bloom.active = true;
        else if(bloomStoredVal == 1) _bloom.active = false;
        bloomUI.value = bloomStoredVal;
        #endregion

        #region Acces filmGrain
        int filmGrainStoredVal = PlayerPrefs.GetInt("FilmGrain");
        if(filmGrainStoredVal == 0) _filmGrain.active = true;
        else if(filmGrainStoredVal == 1) _filmGrain.active = false;
        filmGrainUI.value = filmGrainStoredVal;
        #endregion


        #region Acces master volume
        float masterVol = PlayerPrefs.GetFloat("Master Volume");
        audioMixer.SetFloat("Master", masterVol);
        MasterSlider.value = math.pow(10, (masterVol / 20));
        #endregion

        #region Acces sfx volume
        float sfxVol = PlayerPrefs.GetFloat("SFX Volume");
        audioMixer.SetFloat("SFX", sfxVol);
        SfxSlider.value = math.pow(10, (sfxVol / 20));
        #endregion

        #region Acces music volume
        float musicVol = PlayerPrefs.GetFloat("Music Volume");
        audioMixer.SetFloat("Music", musicVol);
        musicSlider.value = math.pow(10, (musicVol / 20));
        #endregion

        #region Acces ambience volume
        float ambienceVol = PlayerPrefs.GetFloat("Ambience Volume");
        audioMixer.SetFloat("Ambience", ambienceVol);
        ambienceSlider.value = math.pow(10, (ambienceVol / 20));
        #endregion
    }
    
    public void CloseSettings()
    {
        isOpen = false;
        previousMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnDisplayModeChange(int option)
    {
        if(option == 0) _fullscreenMode = FullScreenMode.FullScreenWindow;
        else if(option == 1) _fullscreenMode = FullScreenMode.ExclusiveFullScreen;
        else if(option == 2) _fullscreenMode = FullScreenMode.Windowed;
        else if(option == 3) _fullscreenMode = FullScreenMode.MaximizedWindow;

        Screen.fullScreenMode = _fullscreenMode;

        PlayerPrefs.SetInt("DisplayMode", option);
    }

    private void OnResolutionChange(int option)
    {
        Resolution resolution = _resolutions[option];
        Screen.SetResolution(resolution.width, resolution.height, _fullscreenMode);

        PlayerPrefs.SetInt("Resolution", option);
    }

    // 1 = disabled, 0 = enabled
    void OnBloomChange(int option)
    {
        if(option == 0) _bloom.active = true;
        else if(option == 1) _bloom.active = false;

        PlayerPrefs.SetInt("Bloom", option);
    }

    // 1 = disabled, 0 = enabled
    void OnFilmGrainChange(int option)
    {
        if(option == 0) _filmGrain.active = true;
        else if(option == 1) _filmGrain.active = false;

        PlayerPrefs.SetInt("FilmGrain", option);
    }

    
    void ChangeMasterVolume(float vol)
    {
        float desiredVol = math.log10(vol) * 20;
        audioMixer.SetFloat("Master", desiredVol);

        PlayerPrefs.SetFloat("Master Volume", desiredVol);
    }
    void ChangeSFXVolume(float vol)
    {
        float desiredVol = math.log10(vol) * 20;
        audioMixer.SetFloat("SFX", desiredVol);

        PlayerPrefs.SetFloat("SFX Volume", desiredVol);
    }
    void ChangeMusicVolume(float vol)
    {
        float desiredVol = math.log10(vol) * 20;
        audioMixer.SetFloat("Music", desiredVol);

        PlayerPrefs.SetFloat("Music Volume", desiredVol);
    }
    void ChangeAmbienceVolume(float vol)
    {
        float desiredVol = math.log10(vol) * 20;
        audioMixer.SetFloat("Ambience", desiredVol);

        PlayerPrefs.SetFloat("Ambience Volume", desiredVol);
    }
}
