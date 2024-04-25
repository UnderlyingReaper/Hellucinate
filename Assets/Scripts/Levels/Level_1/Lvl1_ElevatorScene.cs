using CameraShake;
using DG.Tweening;
using UnityEngine;

public class Lvl1_ElevatorScene : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioSource ambienceSource;
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
}
