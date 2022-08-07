using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator 
{
    private Animator animator;
    private string runningAnimStr = "Run"; //HASH
    private string deadAnimStr = "Dead";
    private string jumpAnimStr = "Jump";
    private string slideAnimStr = "Slide";
    private string idleAnimStr = "Idle";

    public PlayerAnimator(Animator animator)
    {
        if (animator)
            this.animator = animator;
    }
    public void SetRunState(bool isRunning)
    {
        animator?.SetBool(runningAnimStr, isRunning);
    }
    public void SetJumpState(bool isJumping)
    {
        animator?.SetBool(jumpAnimStr, isJumping);
    }
    public void SetDeadState(bool isDead)
    {
        animator?.SetBool(deadAnimStr, isDead);
    }
    public void SetSlideState(bool isSliding)
    {
        animator?.SetBool(slideAnimStr, isSliding);
    }
    public void SetIdleState(bool isIdle)
    {
        animator?.SetBool(idleAnimStr, isIdle);
    }
}
