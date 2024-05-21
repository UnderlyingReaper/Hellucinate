using System;
using System.Reflection;
using UnityEngine;

public class SoundScare : MonoBehaviour
{
    public MonoBehaviour mainScript;
    public string eventName; 

    public AudioClip audioClip;
    public float sanityLoseAmount = 1;
    public event EventHandler OnScare;

    AudioSource _audioSource;
    bool _allowTrigger;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _allowTrigger = true;

        EventInfo eventInfo = mainScript.GetType().GetEvent(eventName);
        EventHandler handler = new EventHandler(OnLightSwitch);
        eventInfo.AddEventHandler(mainScript, handler);
    }

    private void OnLightSwitch(object sender, EventArgs e)
    {
        if(!_allowTrigger) return;
        
        _allowTrigger = false;
        OnScare?.Invoke(this, EventArgs.Empty);
        _audioSource.PlayOneShot(audioClip);
    }
}
