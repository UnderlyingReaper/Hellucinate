using System;
using UnityEngine;

public class SoundScare : MonoBehaviour
{
    public AudioClip audioClip;
    public float sanityLoseAmount = 1;

    AudioSource _audioSource;
    bool _allowTrigger;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _allowTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _allowTrigger)
        {
            _audioSource.volume = UnityEngine.Random.Range(0.8f, 1.2f);
            _audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            _audioSource.PlayOneShot(audioClip);

            other.GetComponent<Sanity>().RemoveSanity(sanityLoseAmount);
            _allowTrigger = false;

            Destroy(gameObject, 2);
        }
    }
}
