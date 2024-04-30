using System;
using UnityEngine;

public class SoundScare : MonoBehaviour
{
    public AudioClip audioClip;
    public float sanityLoseAmount = 1;
    public event EventHandler OnScare;

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

            other.GetComponent<Sanity>().LoseSanity(sanityLoseAmount, 1);
            _allowTrigger = false;

            OnScare?.Invoke(this, EventArgs.Empty);

            Destroy(gameObject, 2);
        }
    }
}
