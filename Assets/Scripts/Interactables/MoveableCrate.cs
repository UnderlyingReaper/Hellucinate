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
    Rigidbody2D _rb, _playerRb;


    bool isHeldDown;
    float _timeHeld;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _playerRb = _player.GetComponent<Rigidbody2D>();


        _rb = GetComponent<Rigidbody2D>();
        audioSource.volume = 0;
        canvas.alpha = 0;
    }

    void Update()
    {
        getInputFromPlayer();

        DragObject();

        // Clamp the value of the time held between 0sec - 1sec
        _timeHeld = Mathf.Clamp(_timeHeld, 0, 1);

        // Set the slider value to show how long the player has held it
        slider.value = _timeHeld;

        if(isGrabbed && _rb.velocity.magnitude > 0.1f)
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
            if(isHeldDown) _timeHeld += Time.deltaTime * increaseSpeed;
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
            // Make the crate moveable
            _rb.constraints = RigidbodyConstraints2D.None;

            if(_player.position.x > transform.position.x)
            {
                if(_player.localScale.x == 1) _playerRb.drag = 50;
                else _playerRb.drag = 0;
            }
            else if(_player.position.x < transform.position.x)
            {
                if(_player.localScale.x == -1) _playerRb.drag = 50;
                else _playerRb.drag = 0;
            }

            _rb.velocity = _playerRb.velocity;
        }
        else
        {
            // Make the crate immovable
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _playerRb.drag = 0;
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
        isHeldDown = false;
    }

    public void OnInteractKeyDown()
    {
        isHeldDown = true;
    }
}
