using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbing : MonoBehaviour
{
    [Header("General")]
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float climbAmt;
    public float stepDelay;
    public float ladderEndRange = 1;
    public LayerMask ladderLayer;
    public LayerMask groundLayer;

    [Header("General Bools")]
    public bool isLadderInReach;
    public bool isLadderOnTop;
    public bool isLadderEndNear;
    public bool isOnladder;
    public bool isGroundInReach;
    

    [Header("Colliders")]
    public Vector3 ladderDetectBoxPos;
    public Vector2 ladderDetectBoxSize;
    [Space(15)]
    public Vector3 groundDetectBoxOffset;
    public Vector2 groundDetectBoxSize;
    [Space(15)]
    public Vector3 topDetectBoxOffset;
    public Vector2 topDetectBoxSize;

    public Transform _player;
    PlayerInputManager _playerInputManager;
    Player_Movement _playerMovement;
    Rigidbody2D _rb;
    Ladder _ladder;


    void Start()
    {
        _player = transform.parent;

        _playerMovement = _player.GetComponent<Player_Movement>();
        _rb = _player.GetComponent<Rigidbody2D>();

        _playerInputManager = _player.GetComponent<PlayerInputManager>();
        _playerInputManager.playerInput.Player.Interact.performed += GetOnOffLadder;
    }

    void Update()
    {
        // Check for ladder
        isLadderInReach = Physics2D.OverlapBox(transform.position + ladderDetectBoxPos, ladderDetectBoxSize, 0, ladderLayer);
        // Ground check should be done below the player
        isGroundInReach = Physics2D.OverlapBox(_player.position + groundDetectBoxOffset, groundDetectBoxSize, 0, groundLayer);
        //Check for any ladder on top
        isLadderOnTop = Physics2D.OverlapBox(transform.position + topDetectBoxOffset, topDetectBoxSize, 0, ladderLayer);

        // Get ladder end point
        if(isLadderInReach)
        {
            _ladder = Physics2D.OverlapBox(transform.position + ladderDetectBoxPos, ladderDetectBoxSize, 0, ladderLayer).GetComponent<Ladder>();

            if(Vector3.Distance(_ladder.endPoint.position, transform.position) < ladderEndRange) isLadderEndNear = true;
            else isLadderEndNear = false;
        }
        else
        {
            _ladder = null;
            isLadderEndNear = false;
        }
    }

    void GetOnOffLadder(InputAction.CallbackContext context)
    {
        // Run if ladder is in reach & the player is not on the ladder neither on the end
        if(isLadderInReach && !isOnladder && !isLadderEndNear) // Get on ladderfrom the bottom
        {
            isOnladder = true;
            _playerMovement.enabled = false;
            _rb.gravityScale = 0;

            StartCoroutine(ClimbLadder());
        }
        // Run if ground is in reach & the player is on the ladder but not near the end
        else if(isGroundInReach && isOnladder && !isLadderEndNear) // Get off ladder on the bottom
        {
            isOnladder = false;
            _playerMovement.enabled = true;
            _rb.gravityScale = 4;
        }
        // Run if the player is near the end point & is on the ladder
        else if(isLadderEndNear && isOnladder) // Get off ladder on the top
        {
            _player.position = new Vector3(_ladder.endPoint.position.x, _ladder.endPoint.position.y + 0.3f, _ladder.endPoint.position.z);
            isOnladder = false;
            _playerMovement.enabled = true;
            _rb.gravityScale = 4;
        }
        // Run if the player is not on the ladder but is in reach & is on the top
        else if(isLadderEndNear && !isOnladder && isLadderInReach) // Get on the ladder from the top
        {
            _player.position = _ladder.climbingPos.position;
            isOnladder = true;
            _playerMovement.enabled = false;
            _rb.gravityScale = 0;

            // Flip the player since ur supposed to be facing the ladder
            _player.localScale = new Vector3(-_player.localScale.x, 1, 1);
            _playerMovement.isFacingRight = !_playerMovement.isFacingRight;

            StartCoroutine(ClimbLadder());
        }
    }

    IEnumerator ClimbLadder()
    {
        while(isOnladder)
        {
            // get input from player and calculate the next steps positon
            Vector2 input = _playerInputManager.playerInput.Player.Movement.ReadValue<Vector2>();
            float nextStepPos = input.y * climbAmt;

            // dont let player move down/up if there is any obstruction(or no ladder to step on to) in his way
            if(input.y < 0 && isGroundInReach) Debug.Log("Cant move down");
            else if(input.y > 0 && !isLadderOnTop) Debug.Log("Cant move up");
            else if (input.y != 0)
            {
                _player.DOMoveY(_player.position.y + nextStepPos, stepDelay);
                PlayAudio();
            }

            yield return new WaitForSeconds(stepDelay);
        }
    }

    public void PlayAudio()
    {
        float vol = UnityEngine.Random.Range(0.8f, 1.2f);
        float pitch = UnityEngine.Random.Range(0.8f, 1.2f);

        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(audioClip);
    }


    void OnDrawGizmos()
    {
        // Ladder check gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + ladderDetectBoxPos, ladderDetectBoxSize);

        // Ground check gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_player.position + groundDetectBoxOffset, groundDetectBoxSize);

        // Ladder on top check gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + topDetectBoxOffset, topDetectBoxSize);
    }
}
