using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    [Header("General")]
    public Vector3 centerOffset;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public AudioClip unlockDoorClip, doorLockedClip;
    public float range = 1;
    public CanvasGroup openUI;

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

    float _distance;
    AudioSource _audioSource;
    Inventory_System invSystem;
    Transform _player;
    PlayerInputManager _pInputManager;
    BoxCollider2D _collider;
    SpriteRenderer _sprite;
    ShadowCaster2D _shadowCaster;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pInputManager = _player.GetComponent<PlayerInputManager>();
        _pInputManager.playerInput.Player.Interact.performed += TryOpenDoor;

        invSystem = _player.GetComponent<Inventory_System>();

        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _shadowCaster = GetComponent<ShadowCaster2D>();

        _audioSource = GetComponentInChildren<AudioSource>();

        if(_sprite != null && !isOpen) _sprite.enabled = false;
        if(openUI != null) openUI.alpha = 0;
        if(_shadowCaster != null) _shadowCaster.enabled = true; 
    }

    void Update()
    {
        _distance = Vector3.Distance(_player.position, transform.position + centerOffset);

        if(openUI != null && _distance <= range) openUI.DOFade(1, 1);
        else if(openUI != null && _distance > range) openUI.DOFade(0, 1);
    }

    public void TryOpenDoor(InputAction.CallbackContext context)
    {
        if(_distance > range) return;
        Debug.Log("Not Locked");

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
            }
        }

        // Dont run any code if the door is locked
        if(isLocked) return;

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
        _collider.enabled = false;
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
        _collider.enabled = true;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + centerOffset, range);
    }
}
