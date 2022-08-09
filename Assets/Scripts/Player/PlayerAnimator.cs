using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator 
{
    private Animator animator;
    private int runningHash = Animator.StringToHash("Run");
    private int deadHash = Animator.StringToHash("Dead");
    private int jumpHash = Animator.StringToHash("Jump");
    private int slideHash = Animator.StringToHash("Slide");
    private int idleHash = Animator.StringToHash("Idle");

    public PlayerAnimator(Animator animator)
    {
        if (animator)
            this.animator = animator;
    }
    public void SetRunState(bool isRunning)
    {
        animator?.SetBool(runningHash, isRunning);
    }
    public void SetJumpState(bool isJumping)
    {
        animator?.SetBool(jumpHash, isJumping);
    }
    public void SetDeadState(bool isDead)
    {
        animator?.SetBool(deadHash, isDead);
    }
    public void SetSlideState(bool isSliding)
    {
        animator?.SetBool(slideHash, isSliding);
    }
    public void SetIdleState(bool isIdle)
    {
        animator?.SetBool(idleHash, isIdle);
    }
}
