using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using UnityEngine;

public class Telephone : MonoBehaviour
{
    [Header("General")]
    public bool allowInteraction = false;
    public bool isUsing;
    public float range;

    [Header("Code")]
    public int code;
    public int reverseCode; // not the actual code
    public List<int> codeDigits;
    public float timeDelay;

    [Header("Audio")]
    public AudioSource ringSource;
    public AudioSource staticSource;
    public AudioSource sfxSource, mansVoiceSource;
    public AudioClip pickUp_Clip, putDown_Clip;
    public AudioClip[] number_Clips;

    [Header("Extras?")]
    public MonoBehaviour objectToSubscribe;
    public int noOfInteraction = 0;

    public event EventHandler<CodeGeneratedArgs> OnCodeGenerated;
    public class CodeGeneratedArgs : EventArgs {
        public int code;
    }


    CanvasGroup canvas;
    Transform _player;


    void Start()
    {
        canvas = GetComponentInChildren<CanvasGroup>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        EventInfo eventInfo = objectToSubscribe.GetType().GetEvent("OnScare");
        EventHandler handler = new EventHandler(RingTelephoneEvent);
        eventInfo.AddEventHandler(objectToSubscribe, handler);

        canvas.alpha = 0;
        staticSource.volume = 0;
        ringSource.volume = 0;
    }

    void Update()
    {
        if(!allowInteraction) return;

        float distance = Vector2.Distance(_player.position, transform.position);

        if(distance <= range)
        {
            canvas.DOFade(1, 1);
            
            if(Input.GetKeyDown(KeyCode.E) && !isUsing)
            {
                isUsing = true;
                noOfInteraction++;

                ringSource.volume = 0;
                staticSource.volume = 1;
                if(noOfInteraction == 2) GenerateCode(); // generate another code when player interacts the second time
                PlaySound(pickUp_Clip);
                
                StartCoroutine(PlayMansVoice());
            }
            else if(Input.GetKeyDown(KeyCode.E) && isUsing)
            {
                isUsing = false;

                staticSource.volume = 0;
                PlaySound(putDown_Clip);

                StopCoroutine(PlayMansVoice());
            }
        }
        else canvas.DOFade(0, 2);
    }

    IEnumerator PlayMansVoice()
    {
        if(codeDigits.Count == 0 || codeDigits == null) yield break;

        while(isUsing)
        {
            foreach(int digit in codeDigits)
            {
                yield return new WaitForSeconds(timeDelay);
                if(!isUsing) yield break;
                mansVoiceSource.PlayOneShot(number_Clips[digit]);
            }
            yield return new WaitForSeconds(timeDelay);
        }
    }

    public void GenerateCode()
    {
        code = UnityEngine.Random.Range(123456, 999999);
        if(noOfInteraction == 2) OnCodeGenerated?.Invoke(this, new CodeGeneratedArgs { code = code });

        // Reverse The Code
        string numStr = code.ToString();
        // Convert the string to a character array
        char[] charArray = numStr.ToCharArray();
        // Reverse the character array
        System.Array.Reverse(charArray);
        // Convert the character array back to a string
        string reversedStr = new string(charArray);
        // Convert the reversed string back to an integer
        int.TryParse(reversedStr, out reverseCode);

        // Now seperate the reversed code into digits to use for the sounds
        codeDigits.Clear();
        foreach (char c in reversedStr)
        {
            int digit = int.Parse(c.ToString());
            codeDigits.Add(digit);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        sfxSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        sfxSource.PlayOneShot(clip);
    }

    public void RingTelephoneEvent(object sender, EventArgs e)
    {
        StartCoroutine(RingTelephone());
        allowInteraction = true;
        GenerateCode();
    }
    IEnumerator RingTelephone()
    {
        yield return new WaitForSeconds(2);
        ringSource.volume = 1;
    }
}
