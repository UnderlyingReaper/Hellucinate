using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Item : MonoBehaviour, IInteractible
{
    public string item_ID;
    public AudioSource source;

    [Tooltip("Leave empty if u dont want to use a custome sprite")]
    public Sprite customeSprite;
    public Vector2 customeSize = new Vector2(100, 100);

    public event EventHandler<ItemPickUpInfo> OnItemPickedUp;
    public class ItemPickUpInfo : EventArgs{
        public Inventory_System inventorySystem;
    }

    [Header("Player Speech")]
    public string displayText;
    public float displayTime;

    [Header("Importance Of Item")]
    [Tooltip("Everything can be left empty if the item is not important")]
    public bool isImportant;
    public Light2D lightSource;
    public float minVal, maxVal, delayTime;


    CanvasGroup _canvasGroup;
    bool _isDissolving;
    Material _material;
    SpriteRenderer _spriteRenderer;
    Transform _player;
    PlayerTextDisplay _playerTextDisplay;
    Inventory_System _inventorySystem;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTextDisplay = _player.GetComponent<PlayerTextDisplay>();

        _inventorySystem = _player.GetComponent<Inventory_System>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        _material = GetComponent<SpriteRenderer>().material;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _canvasGroup.alpha = 0;

        if(isImportant) StartCoroutine(LightAnimation());
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!gameObject.activeSelf) return;
        if(_isDissolving) return;

        // Check if there is a empty slot available and then add item to the slot
        if(_inventorySystem.CheckForEmptySlot())
        {
            _inventorySystem.AddItem(item_ID, customeSprite ? customeSprite : _spriteRenderer.sprite, _spriteRenderer.color, customeSize);
            PickupItem();

            if(displayText != "") StartCoroutine(_playerTextDisplay.DisplayPlayerText(displayText, displayTime));
        }
        else Debug.Log("No space available in inventory");
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
        OnItemPickedUp?.Invoke(this, new ItemPickUpInfo { inventorySystem = _inventorySystem});
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

    public void HideCanvas()
    {
        if(!_isDissolving) _canvasGroup.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        if(!_isDissolving) _canvasGroup.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
