using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveableCrate : MonoBehaviour
{
    [Header("General")]
    public bool isGrabbed;
    public float range = 2;
    public float increaseSpeed = 1;
    public float decreaseSpeed = 2;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("UI")]
    public CanvasGroup canvas;
    public Slider slider;


    Transform _player;
    PlayerInputManager _playerInputManager;
    Rigidbody2D _rb, _playerRb;


    bool isHeldDown;
    float _timeHeld;
    float _distance;



    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Interact.started += OnButtonDown;
        _playerInputManager.playerInput.Player.Interact.canceled += OnButtonUp;

        _playerRb = _player.GetComponent<Rigidbody2D>();


        _rb = GetComponent<Rigidbody2D>();
        audioSource.volume = 0;
        canvas.alpha = 0;
    }

    void Update()
    {
        _distance = Vector2.Distance(_player.position, transform.position);
        if(_distance > range) return;

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

    public void OnButtonDown(InputAction.CallbackContext context) => isHeldDown = true;
    public void OnButtonUp(InputAction.CallbackContext context) => isHeldDown = false;

    public void getInputFromPlayer()
    {
        // Check if player is in range
        if(_distance <= range)
        {
            canvas.DOFade(1, 1);

            // If player is in range, get input from the button to see if he wants to grab or not
            if(isHeldDown) _timeHeld += Time.deltaTime * increaseSpeed;
            else  _timeHeld -= Time.deltaTime * decreaseSpeed;
            
            // If player has held the button for 1sec, he will start grabbing it, else not
            if(_timeHeld >= 1) isGrabbed = true;
            else isGrabbed = false;
            
        }
        else
        {
            // player is out of range, set the counter back to 0sec
            canvas.DOFade(0, 1);
            _timeHeld -= Time.deltaTime * decreaseSpeed;
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
}
