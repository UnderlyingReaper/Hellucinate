using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour, IInteractible
{
    [Header("General")]
    public Vector3 centerOffset;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public AudioClip unlockDoorClip, doorLockedClip;
    public CanvasGroup openUI;

    [Header("Player Text Display")]
    public string playerTxt;
    public float displayDuration;

    [Header("Door Settings")]
    [Tooltip("These settings are not used by the teleport settings")]
    public bool isOpen = false;
    public float duration = 1;
    public float doorCloseSize = -0.01f;

    [Header("Teleport Settings")]
    public bool doesTeleport;
    public Transform teleportLocation;
    public CanvasGroup fadeImg;

    [Header("Lock Settings")]
    public bool isLocked = false;
    public string keyID = "NO_KEY_ID";


    AudioSource _audioSource;
    Inventory_System invSystem;
    Transform _player;
    public BoxCollider2D collider1;
    SpriteRenderer _sprite;
    ShadowCaster2D _shadowCaster;
    PlayerTextDisplay _playerTextDisplay;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTextDisplay = _player.GetComponent<PlayerTextDisplay>();

        invSystem = _player.GetComponent<Inventory_System>();

        _sprite = GetComponent<SpriteRenderer>();
        _shadowCaster = GetComponent<ShadowCaster2D>();

        _audioSource = GetComponentInChildren<AudioSource>();

        if(_sprite != null && !isOpen) _sprite.enabled = false;
        if(openUI != null) openUI.alpha = 0;
        if(_shadowCaster != null) _shadowCaster.enabled = true; 
    }

    IEnumerator TeleportPlayer()
    {   
        fadeImg.DOFade(1, 0.9f);
        _audioSource.PlayOneShot(openDoorClip);
        
        yield return new WaitForSeconds(.9f);
        _player.position = teleportLocation.position;
        yield return new WaitForSeconds(0.3f);

        _audioSource.PlayOneShot(closeDoorClip);
        fadeImg.DOFade(0, 1f);
    }

    public void OpenDoor()
    {
        isOpen = true;
        if(_shadowCaster) _shadowCaster.enabled = false;
        transform.DOScale(Vector3.one, duration);
        collider1.enabled = false;
        _sprite.enabled = true;
        _audioSource.PlayOneShot(openDoorClip);
    }

    IEnumerator CloseDoor()
    {
        isOpen = false;
        transform.DOScale(new Vector3(doorCloseSize, 1, 1), duration);

        _audioSource.PlayOneShot(closeDoorClip);

        yield return new WaitForSeconds(duration);
        
        if(_shadowCaster) _shadowCaster.enabled = true;
        _sprite.enabled = false;
        collider1.enabled = true;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(isLocked)
        {
            bool doesHaveKey = invSystem.CheckForItem(keyID);

            if(doesHaveKey)
            {
                isLocked = false;
                invSystem.RemoveItem(keyID);
                _audioSource.PlayOneShot(unlockDoorClip);
                Debug.Log("Item Used");
            }
            else
            {
                _audioSource.PlayOneShot(doorLockedClip);
                StartCoroutine(_playerTextDisplay.DisplayPlayerText(playerTxt, displayDuration));
                return;
            }
        }

        // If door does teleport, than run this code only and return back;
        if(doesTeleport)
        {
            StartCoroutine(TeleportPlayer()); // Teleport player
            return;
        }

        // If door does not teleport, than run this code;
        if(!isOpen) // Open door
        {
            OpenDoor();
        }
        else if(isOpen) // Close door
        {
            StartCoroutine(CloseDoor());
        }
    }

    public void HideCanvas()
    {
        if(openUI != null) openUI.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        if(openUI != null) openUI.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {
        
    }

    public void OnInteractKeyDown()
    {

    }
}
