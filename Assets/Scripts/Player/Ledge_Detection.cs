using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ledge_Detection : MonoBehaviour
{
    [Header("General")]
    public bool ledgeDetected;
    public LayerMask groundLayerMask;
    
    public Vector3 offset;
    public PlayerAnimator playerAnim;
    public bool _canDetect;

    [Header("Colliders")]
    public float radius;
    public Vector3 boxOffset;
    public Vector2 boxSize;

    Transform _player;
    PlayerInputManager _playerInputManager;
    


    void Start()
    {
        _player = transform.parent;
        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.LedgeClimb.performed += ClimbLedge;
    }

    void Update()
    {
        _canDetect = !Physics2D.OverlapBox(transform.position + boxOffset, boxSize, 0, groundLayerMask);

        if(_canDetect)
            ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, groundLayerMask);
        if(!_canDetect) ledgeDetected = false;
    }

    public void ClimbLedge(InputAction.CallbackContext context)
    {
        if(ledgeDetected) StartCoroutine(StartClimbLedge());
    }

    IEnumerator StartClimbLedge()
    {
        playerAnim.climbingLedge = true;
        yield return new WaitForSeconds(playerAnim.ledgeClimbDuration);

        _player.transform.position = transform.position + offset;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireCube(transform.position + boxOffset, boxSize);
    }
}
