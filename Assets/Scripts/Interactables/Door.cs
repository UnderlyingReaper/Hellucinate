using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Door : MonoBehaviour
{
    [Header("General")]
    public Vector3 centerOffset;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
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


    AudioSource _audioSource;
    Inventory_System invSystem;
    Transform _player;
    BoxCollider2D _collider;
    SpriteRenderer _sprite;
    ShadowCaster2D _shadowCaster;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
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
        float distance = Vector3.Distance(_player.position, transform.position + centerOffset);

        if(isLocked && Input.GetKeyDown(KeyCode.E) && distance <= range)
        {
            bool doesHaveKey = invSystem.CheckForItem(keyID);

            if(doesHaveKey)
            {
                isLocked = false;
                invSystem.RemoveItem(keyID);
                Debug.Log("Item Used");
            }
        }

        if(openUI != null && distance <= range) openUI.DOFade(1, 1);
        else if(openUI != null && distance > range)
        {
            openUI.DOFade(0, 1);
            return;
        }

        // Dont run any code if the door is locked
        if(isLocked) return;

        // If door does teleport, than run this code only and return back;
        if(doesTeleport && distance <= range && Input.GetKeyDown(KeyCode.E) && !isLocked)
        {
            StartCoroutine(TeleportPlayer()); // Teleport player
            return;
        }

        // If door does not teleport, than run this code;
        if(distance <= range && Input.GetKeyDown(KeyCode.E) && !isLocked && !isOpen) // Open door
        {
            isOpen = true;
            if(_shadowCaster) _shadowCaster.enabled = false;
            transform.DOScale(Vector3.one, duration);
            _collider.enabled = false;
            _sprite.enabled = true;
            _audioSource.PlayOneShot(openDoorClip);
        }
        else if(distance <= range && Input.GetKeyDown(KeyCode.E) && !isLocked && isOpen) // Close door
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
