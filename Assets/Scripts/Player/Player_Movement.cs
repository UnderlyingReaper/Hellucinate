using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public bool allow = true;
    public Rigidbody2D rb;
    public Animator anim;
    public float speed;
    public bool isFacingRight;


    float _horizontal;


    void Update()
    {
        if(!allow) return;

        _horizontal = Input.GetAxisRaw("Horizontal");

        HandleAnimations();

        Flip();
    }
    void FixedUpdate()
    {
        if(!allow) return;

        rb.velocity = new Vector2(_horizontal * speed, rb.velocity.y);
    }

    void HandleAnimations()
    {
        if(_horizontal > 0 || _horizontal < 0)
            anim.SetBool("isWalking", true);
        
        else
            anim.SetBool("isWalking", false);
    }

    void Flip ()
    {
        if(isFacingRight && _horizontal < 0 || !isFacingRight && _horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
