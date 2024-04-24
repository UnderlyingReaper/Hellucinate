using UnityEngine;

public class Lvl1_ElevatorScene : MonoBehaviour
{
    public AudioSource sfxSource;


    public void PlaySFXSound(AudioClip clip)
    {
        sfxSource.volume = Random.Range(0.8f, 1.2f);
        sfxSource.pitch = Random.Range(0.8f, 1.2f);

        sfxSource.PlayOneShot(clip);
    }
}
