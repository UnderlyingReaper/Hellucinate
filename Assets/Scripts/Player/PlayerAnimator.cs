using Unity.VisualScripting;
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

    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Walk = Animator.StringToHash("Walk");
    public static readonly int LedgeClimb = Animator.StringToHash("LedgeClimb");
    public static readonly int FallOnKnee = Animator.StringToHash("FallOnKnee");

    int _currState;
    float _lockedTimer;


    void Start()
    {
        anim.CrossFade(Idle, 0, 0);
        anim.CrossFade(Idle, 0, 1);
        _currState = Idle;
    }
    void Update()
    {
        var state = GetState();

        if(state == _currState) return;
        anim.CrossFade(state, 0, 0);
        anim.CrossFade(state, 0, 1);
        _currState = state;
    }

    int GetState()
    {
        if(Time.time < _lockedTimer) return _currState;

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