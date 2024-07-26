using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public bool allow = true;
    public bool lockRotation;
    public Rigidbody2D rb;
    public PlayerAnimator anim;
    public float speed;
    public bool isFacingRight;


    Vector2 _movement;
    float _speedToUse;
    PlayerInputManager _playerInputManager;



    void Start()
    {
        _speedToUse = speed;
        anim = GetComponent<PlayerAnimator>();

        _playerInputManager = GetComponent<PlayerInputManager>();
    }
    
    void Update()
    {
        if(!allow)
        {
            anim.isWalking = false;
            return;
        }
        if(anim.climbingLedge) return;

        _movement = _playerInputManager.playerInput.Player.Movement.ReadValue<Vector2>();

        HandleAnimations();

        if(!lockRotation) Flip();
    }
    void FixedUpdate()
    {
        if(!allow) return;

        rb.velocity = new Vector2(_movement.x * _speedToUse, rb.velocity.y);
    }

    void HandleAnimations()
    {
        if(_movement.x > 0 || _movement.x < 0)
            anim.isWalking = true;
        
        else
            anim.isWalking = false;
    }

    void Flip ()
    {
        if(isFacingRight && _movement.x < 0 || !isFacingRight && _movement.x > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void ManipulatePlayerSpeed(float multiplier)
    {
        _speedToUse = speed * multiplier;
    }
}
