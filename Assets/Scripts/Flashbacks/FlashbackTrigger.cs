using System;
using System.Collections;
using UnityEngine;

public class FlashbackTrigger : MonoBehaviour
{
    public bool allow = true;
    public AudioSource source;
    public AudioClip[] clips;
    public float[] delayBetweenClips;
    public float defaultDelay = 0;



    public event EventHandler<FlashTriggerInfo> OnFlashTrigger;
    public class FlashTriggerInfo : EventArgs {
        public bool isStart;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(!allow) return;
        if(other.CompareTag("Player"))
        {
            allow = false;
            OnFlashTrigger?.Invoke(this, new FlashTriggerInfo{ isStart = true });
            StartCoroutine(PlayFlashBack());
        }
    }

    IEnumerator PlayFlashBack()
    {
        yield return new WaitForSeconds(2*0.25f);

        for(int i = 0; i < clips.Length; i++)
        {
            source.PlayOneShot(clips[i]);

            yield return new WaitWhile(() => source.isPlaying);

            if(delayBetweenClips != null) yield return new WaitForSeconds(delayBetweenClips[i]);
            else yield return new WaitForSeconds(defaultDelay);
        }

        yield return new WaitForSeconds(5);

        OnFlashTrigger?.Invoke(this, new FlashTriggerInfo{ isStart = false });
    }
}
