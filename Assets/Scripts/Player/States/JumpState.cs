using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JumpState : MovingState
{
    private AnimationCurve deltaYCurve;
    private float expiredTime = 0;
    private float previousDeltaY = 0;
    private float jumpHeight;
    private float internalJumpTime = 0.45f;
    public JumpState(PlayerStateMachine playerStateMachine, AnimationCurve curve) : base(playerStateMachine)
    {
        deltaYCurve = curve;
        jumpHeight = playerData.JumpHeight;
    }
    public override void OnStateEnter()
    {
        playerSM.PlayJumpingAnimation(true);
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,from: playerSM.RightHandRigWeight, to: 0.85f,timeToChange: 0.1f);
    }

    public override void OnStateExit()
    {
        playerSM.PlayJumpingAnimation(false);      
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,1);
        EndJump();
    }
    public override void Tick()
    { 
       Jump();
       base.Tick();
    } 
    public void Jump()
    {
        expiredTime += Time.deltaTime;
        float jumpProgress = expiredTime;
        float deltaY = deltaYCurve.Evaluate(jumpProgress) * jumpHeight;
        float diff = deltaY - previousDeltaY;
        previousDeltaY = deltaY;
        playerSM.VerticalDeltaPosition = diff;
        if (jumpProgress > internalJumpTime)
        {  
            playerSM.SetState(playerSM.PlayerGroundState);
        }
    }
    private void EndJump()
    {
        previousDeltaY = 0;
        expiredTime = 0;
        playerSM.VerticalDeltaPosition = 0;
        playerSM.VerticalDeltaPosition = 0;
    }
}
