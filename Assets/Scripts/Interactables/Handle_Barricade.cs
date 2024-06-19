using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Handle_Barricade : MonoBehaviour, IInteractible
{
    public bool isInteractive = true;
    public bool doesRemoveItem = true;
    public string itemID = "Crowbar";
    [Space(20)]

    public float timeToRemoveOne;
    public float increaseMp, decreaseMp;
    public float delayBetweenEachPlank;
    [Space(20)]
    
    public AudioSource audioSource;
    public AudioClip wooddropClip;
    public float soundDelay = 0;
    [Space(20)]

    public List<Rigidbody2D> activePlanks;
    public CanvasGroup canvas;
    public Slider slider;

    public event EventHandler OnBarricadesRemoved;


    Transform _player;
    float _timeHeld;
    bool _allowToBreak = true;
    bool _isHeldDown;
    



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        slider.maxValue = timeToRemoveOne;
        HideCanvas();

        for(int i = 0; i < activePlanks.Count; i++) activePlanks[i].gravityScale = 0;
    }

    void Update()
    {
        if(!isInteractive) return;

        if(activePlanks.Count == 0)
        {   
            isInteractive = false;
            if(doesRemoveItem) _player.GetComponent<Inventory_System>().RemoveItem(itemID);
            canvas.DOFade(0, 1);
            Destroy(gameObject, 1.01f);
            OnBarricadesRemoved?.Invoke(this, EventArgs.Empty);
            return;
        }

        if(_isHeldDown)
        {
            bool doesHaveItem = _player.GetComponent<Inventory_System>().CheckForItem(itemID);

            if(!doesHaveItem) return;
            if(!_allowToBreak) return;

            _timeHeld += Time.deltaTime * increaseMp;
            
            if(_timeHeld >= timeToRemoveOne)
            {
                Debug.Log("e");
                _timeHeld = 0;
                StartCoroutine(RemoveOne());
            }
        }
        else
        {
            _timeHeld -= Time.deltaTime * decreaseMp;
        }

        _timeHeld = Mathf.Clamp(_timeHeld, 0, timeToRemoveOne);
        slider.value = _timeHeld;
    }

    public IEnumerator RemoveOne()
    {
        _allowToBreak = false;
        DOVirtual.Float(_timeHeld, 0, decreaseMp, value => { _timeHeld = value; });
        DOVirtual.Float(slider.value, 0, decreaseMp, value => { slider.value = value; });

        activePlanks[0].gravityScale = 1;
        
        SpriteRenderer planksSprite = activePlanks[0].GetComponent<SpriteRenderer>();
        Color transparentColor = new Color(planksSprite.color.r, planksSprite.color.g, planksSprite.color.b, 0);

        yield return new WaitForSeconds(soundDelay);
        PlaySound(wooddropClip);
        yield return new WaitForSeconds(1 - soundDelay);

        DOVirtual.Color(planksSprite.color, transparentColor, 1, value => { planksSprite.color = value; });
        planksSprite.GetComponent<BoxCollider2D>().enabled = false;
        Destroy(activePlanks[0], 1.01f);
        activePlanks.Remove(activePlanks[0]);
        
        yield return new WaitForSeconds(delayBetweenEachPlank);

        _allowToBreak = true;
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        audioSource.PlayOneShot(clip);
    }

    public void Interact()
    {
        
    }

    public void HideCanvas()
    {
        canvas.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        canvas.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {
        _isHeldDown = false;
    }

    public void OnInteractKeyDown()
    {
        _isHeldDown = true;
    }
}
