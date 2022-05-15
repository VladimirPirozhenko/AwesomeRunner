using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class JumpState : MovingState
{
    private AnimationCurve deltaYCurve;
    private PlayerAnimator animator;
    private float expiredTime = 0;
    private float previousDeltaY = 0;
    private float jumpHeight;
    public JumpState(Player player, PlayerController collider, AnimationCurve curve,PlayerAnimator animator) : base(player,collider)
    {
        this.animator = animator;
        deltaYCurve = curve;
        jumpHeight = player.PlayerData.JumpHeight;
    }
    public override void OnStateEnter()
    {
        animator.SetJumpState(true);
    }

    public override void OnStateExit()
    {
        animator.SetJumpState(false);
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
        player.VerticalDeltaPosition = diff;
        float jumpTime = 0.5f;
        if (jumpProgress > jumpTime)
        {
            expiredTime = 0;
            player.VerticalDeltaPosition = 0;
            player.PlayerStateMachine.SetState(player.PlayerGroundState);
        }
    }
    private void EndJump()
    {
        previousDeltaY = 0;
        expiredTime = 0;
        player.VerticalDeltaPosition = 0;
    }
}
