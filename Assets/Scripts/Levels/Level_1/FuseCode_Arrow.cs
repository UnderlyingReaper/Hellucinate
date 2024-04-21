using System;
using TMPro;
using UnityEngine;

public class FuseCode_Arrow : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public ActionPerformed actionToPerform;

    public event EventHandler OnValyeChange;
    
    public enum ActionPerformed
    {
        IncreaseValue = 1,
        DecreaseValue = -1
    }

    AudioSource _source;

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }


    void OnMouseDown()
    {
        int currNumber = int.Parse(txt.text);

        currNumber += (int)actionToPerform;
        if(currNumber == 10) currNumber = 0;
        else if(currNumber == -1) currNumber = 9;

        txt.text = currNumber.ToString();
        PlaySound();
        
        OnValyeChange?.Invoke(this, EventArgs.Empty);
    }

    void PlaySound()
    {
        _source.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        _source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        _source.PlayOneShot(_source.clip);
    }
}
