using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : MovingState
{
    private float expiredTime = 0;
    private float slideDuration = 0.9f;
    public SlideState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {}
    public override void OnStateEnter()
    {
        playerSM.SetAnimatorSlideState(true);
        playerSM.ChangeColliderCenter(playerSM.DefaultColliderCenter / 2);
        playerSM.ChangeColliderHeight(playerSM.DefaultColliderHeight / 2);
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,0);
        //WeaponController.canShoot = false;
    }
    public override void OnStateExit()
    {
        playerSM.SetAnimatorSlideState(false);
        playerSM.VerticalDeltaPosition = 0;
        expiredTime = 0;
        playerSM.ResetColliderToDefault();
       // WeaponController.canShoot = true;
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,1);

    }
    public override void Tick()
    { 
        Slide();
        base.Tick();  
    }
    public void Slide()
    {
        expiredTime += Time.deltaTime;
        float slideProgress = expiredTime / slideDuration;
        if (slideProgress > slideDuration)
        {
            expiredTime = 0;
            playerSM.SetState(playerSM.PlayerGroundState);
        }
    }
}
