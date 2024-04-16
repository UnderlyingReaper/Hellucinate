using System.Collections;
using UnityEngine;

public class WaterDrip : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float delay;


    void Start()
    {
        StartCoroutine(PlayDripSound());
    }
    IEnumerator PlayDripSound()
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);

            audioSource.volume = Random.Range(0.8f, 1.2f);
            audioSource.pitch = Random.Range(0.8f, 1.2f);

            audioSource.PlayOneShot(audioClip);
        }
    }
}
