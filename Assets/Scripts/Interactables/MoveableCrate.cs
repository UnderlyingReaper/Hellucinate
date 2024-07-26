using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveableCrate : MonoBehaviour, IInteractible
{
    [Header("General")]
    bool isPlayerNear = false;
    public bool isGrabbed;
    public float increaseSpeed = 1;
    public float decreaseSpeed = 2;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("UI")]
    public CanvasGroup canvas;
    public Slider slider;


    Transform _player;
    Rigidbody2D _playerRb;
    Player_Movement _playerMovement;


    bool _isHeldDown;
    float _timeHeld;
    Transform _defaultParent;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerRb = _player.GetComponent<Rigidbody2D>();
        _playerMovement = _player.GetComponent<Player_Movement>();

        _defaultParent = transform.parent;

        audioSource.volume = 0;
        canvas.alpha = 0;
    }

    void Update()
    {
        getInputFromPlayer();

        if(!isPlayerNear) return;

        DragObject();

        // Clamp the value of the time held between 0sec - 1sec
        _timeHeld = Mathf.Clamp(_timeHeld, 0, 1);

        // Set the slider value to show how long the player has held it
        slider.value = _timeHeld;

        if(isGrabbed && _playerMovement.rb.velocity.magnitude > 0.1f)
            DOVirtual.Float(audioSource.volume, 1, 0.25f, value => { audioSource.volume = value; });
        else
            DOVirtual.Float(audioSource.volume, 0, 0.25f, value => { audioSource.volume = value; });
    }

    public void getInputFromPlayer()
    {
        // Check if player is in range
        if(isPlayerNear)
        {
            // If player is in range, get input from the button to see if he wants to grab or not
            if(_isHeldDown) _timeHeld += Time.deltaTime * increaseSpeed;
            else _timeHeld -= Time.deltaTime * decreaseSpeed;
            
            // If player has held the button for 1sec, he will start grabbing it, else not
            if(_timeHeld >= 1) isGrabbed = true;
            else isGrabbed = false;
        }
        else
        {
            _timeHeld -= Time.deltaTime * decreaseSpeed;
            isGrabbed = false;
        }
    }

    public void DragObject()
    {
        // If player has grabbed the crate then move it in the direction he wants
        if(isGrabbed)
        {
            transform.SetParent(_player);
            _playerMovement.lockRotation = true;
            _playerRb.drag = 50;
        }
        else
        {
            // Make the crate immovable
            _playerRb.drag = 0;
            _playerMovement.lockRotation = false;
            transform.SetParent(_defaultParent);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {

    }

    public void HideCanvas()
    {
        canvas.DOFade(0, 1);
        isPlayerNear = false;
    }

    public void ShowCanvas()
    {
        canvas.DOFade(1, 1);
        isPlayerNear = true;
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
