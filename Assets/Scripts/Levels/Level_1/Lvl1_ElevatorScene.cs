using CameraShake;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl1_ElevatorScene : MonoBehaviour
{
    public Animator animController;
    public AudioSource sfxSource;
    public AudioSource ambienceSource;
    public AudioSource gearSource;
    public PerlinShake.Params shakeParams;

    public float minAmp = 0.1f, maxAmp = 1;


    float _amp;


    public void Update()
    {
        CameraShaker.Shake(new PerlinShake(shakeParams, _amp));
    }

    public void PlaySFXSound(AudioClip clip)
    {
        sfxSource.volume = Random.Range(0.3f, 0.7f);
        sfxSource.pitch = Random.Range(0.3f, 0.7f);

        sfxSource.PlayOneShot(clip);
    }
    public void DisableElevatorNoise()
    {
        DOVirtual.Float(ambienceSource.volume, 0, 2, value => { ambienceSource.volume = value; });
    }
    public void DisableGearNoise()
    {
        DOVirtual.Float(gearSource.volume, 0, 2, value => { gearSource.volume = value; });
    }
    public void StartShake(int mp)
    {
        float result = 0;
        if(mp >= 0) result = minAmp + (maxAmp - minAmp)*(mp);
        else if(mp == -1)
        {
            DOVirtual.Float(_amp, 0, 1, value => { _amp = value; });
            return;
        }

        DOVirtual.Float(_amp, result, 1, value => { _amp = value; });
    }

    public void PlayCredits()
    {
        animController.SetTrigger("ShowCredits");
    }
    public void TeleportToNextLvl()
    {
        SceneManager.LoadScene("MainMenu"); // for now teleport back to main menu
    }
}
