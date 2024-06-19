using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour, IInteractible
{
    [Header("General")]
    public bool isEnteractable = true;
    public GameObject itemsInside;
    public Transform door;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public CanvasGroup openUI;

    [Header("Door Settings")]
    public bool isOpen = false;
    public float duration = 1;
    public float doorCloseSoundOffset = 0;
    public float doorOpenSize = -0.01f;

    [Header("Lock Settings")]
    public bool isLocked = false;
    public string keyID = "NO_KEY_ID";


    AudioSource _audioSource;
    Inventory_System invSystem;
    Transform _player;


    void Start()
    {
        openUI.alpha = 0;

        if(!isEnteractable) return;

        _player = GameObject.FindGameObjectWithTag("Player").transform;
        invSystem = _player.GetComponent<Inventory_System>();

        _audioSource = GetComponentInChildren<AudioSource>();

        SetItemsActivateStateTo(false);
        HideCanvas();
    }

    IEnumerator CloseDoor()
    {
        isOpen = false;
        door.DOScaleX(1, duration); 

        yield return new WaitForSeconds(doorCloseSoundOffset);

        _audioSource.PlayOneShot(closeDoorClip);

        yield return new WaitForSeconds(duration - doorCloseSoundOffset);

        SetItemsActivateStateTo(false);
    }

    public void SetItemsActivateStateTo(bool doActivate)
    {
        if(itemsInside == null) return;

        itemsInside.SetActive(doActivate);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(!isEnteractable) return;

        if(isLocked)
        {
            bool doesHaveKey = invSystem.CheckForItem(keyID);

            if(doesHaveKey)
            {
                isLocked = false;
                invSystem.RemoveItem(keyID);
                Debug.Log("Item Used");
            }
            else return;
        }

        // open/close locker
        if(!isOpen) // Open door
        {
            isOpen = true;
            door.DOScaleX(doorOpenSize, duration);
            SetItemsActivateStateTo(true);
            if(openDoorClip != null) _audioSource.PlayOneShot(openDoorClip);
        }
        else if(isOpen) // Close door
        {
            StartCoroutine(CloseDoor());
        }
    }

    public void HideCanvas()
    {
        openUI.DOFade(0, 1);
    }

    public void ShowCanvas()
    {
        openUI.DOFade(1, 1);
    }

    public void OnInteractKeyUp()
    {

    }

    public void OnInteractKeyDown()
    {

    }
}
