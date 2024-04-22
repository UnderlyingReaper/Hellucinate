using System;
using DG.Tweening;
using UnityEngine;

public class Lvl1_fusesSwitching : MonoBehaviour
{
    public AudioSource buzzingSource, lockerUnlock;
    public FuseSwitch[] fuseSwitches;
    public bool isPuzzleComplete = false;
    public Locker lockerToOpen;
    public event EventHandler OnPuzzle2Complete;



    void Start()
    {
        isPuzzleComplete = false;

        foreach(FuseSwitch fuse in fuseSwitches) 
        {
            // This dosent work, idk why
            //int rand = UnityEngine.Random.Range(0, 2);
            //if(rand == 0) fuse.AlterState();

            fuse.OnStateAlter += OnFuseSwitch;
        }
    }

    public void OnFuseSwitch(object sender, EventArgs e)
    {
        CheckAllSwitches();
    } 

    public void CheckAllSwitches()
    {
        bool rAllActive = true;

        foreach(FuseSwitch fuse in fuseSwitches)
        {
            if(fuse.currState == FuseSwitch.CurrentState.On) continue;

            if(fuse.currState == FuseSwitch.CurrentState.Off)
            {
                rAllActive = false;
                return;
            }
        }

        Debug.Log(rAllActive);
        isPuzzleComplete = false;
        lockerToOpen.isLocked = false;
        OnPuzzle2Complete?.Invoke(this, EventArgs.Empty);
        DOVirtual.Float(buzzingSource.volume, 0.5f, 1, value => { buzzingSource.volume = value; });
        lockerUnlock.PlayOneShot(lockerUnlock.clip);

        foreach(FuseSwitch fuse in fuseSwitches)
        {
            fuse.isInteractable = false;
            fuse.enabled = false;
        }
    }
}

