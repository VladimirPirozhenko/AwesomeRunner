using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private string runningAnimStr = "Run";
    private string deadAnimStr = "Dead";
    private string jumpAnimStr = "Jump";
    private string idleAnimStr = "Idle";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetRunState(bool isRunning)
    {
        animator.SetBool(runningAnimStr, isRunning);
    }

    public void SetJumpState(bool isJumping)
    {
        animator.SetBool(jumpAnimStr, isJumping);
    }

    private void Update()
    {
        
    }
}
