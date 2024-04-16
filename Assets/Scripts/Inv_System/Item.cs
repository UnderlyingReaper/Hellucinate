using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Item : MonoBehaviour
{
    public string item_ID;
    public float range;
    public AudioSource source;

    [Tooltip("Leave empty if u dont want to use a custome sprite")]
    public Sprite customeSprite;


    public event EventHandler OnItemPickedUp;

    [Header("Importance Of Item")]
    [Tooltip("Everything can be left empty if the item is not important")]
    public bool isImportant;
    public Light2D lightSource;
    public float minVal, maxVal, delayTime;


    CanvasGroup _canvasGroup;
    bool _isDissolving;
    Material _material;

    Transform _player;



    void Start()
    {
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _material = GetComponent<SpriteRenderer>().material;
        _canvasGroup.alpha = 0;

        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if(isImportant) StartCoroutine(LightAnimation());
    }

    void Update()
    {
        // If shader is dissolving, it means its being picked up so no need to run the code
        if(_isDissolving) return;

        // Check if player is in range
        if(Vector3.Distance(_player.position, transform.position) < range)
        {
            _canvasGroup.DOFade(1, 1f);
            
            // Add Item to inv if the player presses E when in range
            if(Input.GetKeyDown(KeyCode.E))
            {
                Inventory_System inventory_System = _player.GetComponent<Inventory_System>();

                // Check if there is a empty slot available and then add item to the slot
                if(inventory_System.CheckForEmptySlot())
                {
                    inventory_System.AddItem(item_ID, customeSprite ? customeSprite : GetComponent<SpriteRenderer>().sprite);
                    PickupItem();
                }
                else Debug.Log("No space available in inventory");
            }
        }
        else _canvasGroup.DOFade(0, 1f);
    }

    IEnumerator LightAnimation()
    {
        while(true)
        {
            // If shader is dissolving, it means its being picked up so no need to run the code
            if(_isDissolving) break;
            DOVirtual.Float(lightSource.intensity, minVal, delayTime, value => {lightSource.intensity = value; });

            yield return new WaitForSeconds(delayTime);

            if(_isDissolving) break;
            DOVirtual.Float(lightSource.intensity, maxVal, delayTime, value => {lightSource.intensity = value; });
            
            yield return new WaitForSeconds(delayTime);
        }
    }

    public void PickupItem()
    {
        // Send a Trigger that the item has been picked up
        OnItemPickedUp?.Invoke(this, EventArgs.Empty);
        PlayPickupSound(); // play sound
        
        // Start desolving the shader
        _isDissolving = true;
        if(_material != null) DOVirtual.Float(_material.GetFloat("_Fade"), 0, 2, value => {_material.SetFloat("_Fade", value); });

        // Dmin the lights if there r any
        if(isImportant) DOVirtual.Float(lightSource.intensity, 0, 2, value => {lightSource.intensity = value; });

        // Hide canvas
        _canvasGroup.DOFade(0, 1f);
        
        // Destroy the item
        Destroy(gameObject, 2.1f);
    }

    public void PlayPickupSound()
    {
        source.volume = UnityEngine.Random.Range(0.8f, 1.2f);
        source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        source.PlayOneShot(source.clip);
    }
}
