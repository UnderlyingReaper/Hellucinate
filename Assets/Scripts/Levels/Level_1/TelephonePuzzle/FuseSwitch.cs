using System;
using CameraShake;
using DG.Tweening;
using UnityEngine;

public class FuseSwitch : MonoBehaviour
{
    public bool isInteractable = true;
    public CurrentState currState;
    public FuseSwitch switchToAlter;
    public bool alterWhenOn, alterWhenOff;

    public event EventHandler OnStateAlter;

    public enum CurrentState {
        On,
        Off
    }

    AudioSource _source;
    float targetYPos;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }


    void OnMouseDown()
    {
        if(!isInteractable) return;
        AlterState();
    }

    public void AlterState()
    {
        PlaySound();
        DOTween.Kill(transform);
        targetYPos = -transform.localScale.y;
        transform.DOScaleY(targetYPos, 0.3f);

        if(targetYPos < 0) currState = CurrentState.Off;
        else if (targetYPos > 0) currState = CurrentState.On;

        if(switchToAlter != null)
        {
            if(alterWhenOn && currState == CurrentState.On) switchToAlter.AlterState();
            else if(alterWhenOff && currState == CurrentState.Off) switchToAlter.AlterState();
        }
        
        OnStateAlter?.Invoke(this, EventArgs.Empty);
    }

    private void PlaySound()
    {
        _source.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        _source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        _source.PlayOneShot(_source.clip);
    }
}
