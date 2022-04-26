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
    private float jumpDuration = 1;
    private float jumpHeight = 10;
    public JumpState(Player player,CharacterController controller, AnimationCurve curve,PlayerAnimator animator) : base(player,controller)
    {
        this.animator = animator;
        deltaYCurve = curve;
    }
    public override void OnStateEnter()
    {
        animator.SetJumpState(true);
    }

    public override void OnStateExit()
    {
        animator.SetJumpState(false);
    }
    public override void Tick()
    { 
       Jump();
       base.Tick();
    } 
    public void Jump()
    {
        expiredTime += Time.deltaTime;
        float jumpProgress = expiredTime / jumpDuration;
        float deltaY = deltaYCurve.Evaluate(jumpProgress) * jumpHeight;
        float diff = deltaY - previousDeltaY;
        previousDeltaY = deltaY;
        player.VerticalDeltaPosition = diff;
        if (jumpProgress > 0.5 && characterController.isGrounded)
        {
            expiredTime = 0;
            player.VerticalDeltaPosition = 0;
            player.StateMachine.SetState(player.PlayerGroundState);
        }
    }
}
