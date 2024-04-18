using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sockets : MonoBehaviour
{
    public AudioClip electricShock_Clip;
    public Color color;

    public EventHandler OnConnected;
    AudioSource _audioSource;

    public enum Color {
        Red,
        Blue,
        Green,
        Yellow,
    }

    void  Start() => _audioSource = GetComponent<AudioSource>();

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other);

        if(other.GetComponent<PointControl>() != null)
        {
            PointControl pointControl = other.GetComponent<PointControl>();

            if((int)pointControl.color == (int)color && pointControl.type == PointControl.Type.End)
            {
                if(Input.GetKeyUp(KeyCode.Mouse0))
                {
                    OnConnected?.Invoke(this, EventArgs.Empty);
                    pointControl.transform.position = transform.position;
                    PlaySound(electricShock_Clip);
                    pointControl.enabled = false;
                    enabled = false;
                }
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        _audioSource.volume = UnityEngine.Random.Range(0.3f, 0.7f);
        _audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        _audioSource.PlayOneShot(clip);
    }
}
