using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator anim;

    [Header("ANimation Durations")]
    public float ledgeClimbDuration;

    [Header("Parameters")]
    public bool isWalking;
    public bool climbingLedge;

    public bool fallOnKnees;

    public bool isLadderStep1;
    public bool isLadderStep2;

    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Walk = Animator.StringToHash("Walk");
    public static readonly int LedgeClimb = Animator.StringToHash("LedgeClimb");
    public static readonly int FallOnKnee = Animator.StringToHash("FallOnKnee");

    public static readonly int LadderStep1 = Animator.StringToHash("LadderStep1");
    public static readonly int LadderStep2 = Animator.StringToHash("LadderStep2");

    int _currState;
    float _lockedTimer;
    Player_Movement _playerMovement;


    void Start()
    {
        _playerMovement = GetComponent<Player_Movement>();

        anim.CrossFade(Idle, 0, 0);
        anim.CrossFade(Idle, 0, 1);
        _currState = Idle;
    }
    void Update()
    {
        var state = GetState();
        
        if(_playerMovement.lockRotation && ((_playerMovement.rb.velocityX > 0 && !_playerMovement.isFacingRight) || (_playerMovement.rb.velocityX < 0 && _playerMovement.isFacingRight)))
        {
            anim.SetFloat("Speed", -1);
        }
        else anim.SetFloat("Speed", 1);


        if(state == _currState) return;

        anim.CrossFade(state, 0, 0);
        anim.CrossFade(state, 0, 1);
        _currState = state;
    }

    int GetState()
    {
        if(Time.time < _lockedTimer) return _currState;

        if(isLadderStep1) return LadderStep1;
        else if(isLadderStep2) return LadderStep2;

        if(fallOnKnees) return FallOnKnee;

        if(climbingLedge)
        {
            climbingLedge = false;
            return LockState(LedgeClimb, ledgeClimbDuration);
        }

        if(isWalking) return Walk;
        else return Idle;
    }

    int LockState(int state, float lockDuration)
    {
        _lockedTimer = Time.time + lockDuration;
        return state;
    }
}