using UnityEngine;

public class Player_SFX : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource footStep_Source;
    public AudioSource animation_Source;

    [Header("Clips")]
    public AudioClip footStep_Clip;


    public void PlayFootStepSound()
    {
        footStep_Source.volume = Random.Range(0.2f, 0.4f);
        footStep_Source.pitch = Random.Range(0.8f, 1.2f);

        footStep_Source.PlayOneShot(footStep_Clip);
    }

    public void PlayAnimationSound(AudioClip clip)
    {
        animation_Source.volume = Random.Range(0.8f, 1.2f);
        animation_Source.pitch = Random.Range(0.8f, 1.2f);

        animation_Source.PlayOneShot(clip);
    }
}
