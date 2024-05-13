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

    void Start()
    {
        volume.profile.TryGet(out _bloom);
        volume.profile.TryGet(out _filmGrain);

        GetSettings();

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
