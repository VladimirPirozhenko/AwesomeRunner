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

    //List<AnimationClip> animationClips = new List<AnimationClip>();
    public PlayerAnimator(Animator playerAnimator)
    {
        this.animator = playerAnimator;//GetComponent<Animator>();
        //for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        //{
        //    AnimationClip clip = animator.runtimeAnimatorController.animationClips[i];

        //    AnimationEvent animationStartEvent = new AnimationEvent();
        //    animationStartEvent.time = 0;
        //    animationStartEvent.functionName = "AnimationStartHandler";
        //    animationStartEvent.stringParameter = clip.name;

        //    AnimationEvent animationEndEvent = new AnimationEvent();
        //    animationEndEvent.time = clip.length;
        //    animationEndEvent.functionName = "AnimationCompleteHandler";
        //    animationEndEvent.stringParameter = clip.name;

        //    clip.AddEvent(animationStartEvent);
        //    clip.AddEvent(animationEndEvent);
        //}
    }
    public void SetRunState(bool isRunning)
    {
        animator.SetBool(runningAnimStr, isRunning);
    }
    public void SetJumpState(bool isJumping)
    {
        animator.SetBool(jumpAnimStr, isJumping);
    }
    public void SetDeadState(bool isDead)
    {
        animator.SetBool(deadAnimStr, isDead);
    }
    public void SetSlideState(bool isSliding)
    {
        animator.SetBool(slideAnimStr, isSliding);
    }
    //public void AnimationStartHandler(string name)
    //{
    //    Debug.Log($"{name} animation start.");
    //    OnAnimationStart?.Invoke(name);
    //}
    //public void AnimationCompleteHandler(string name)
    //{
    //    Debug.Log($"{name} animation start.");
    //    OnAnimationComplete?.Invoke(name);
    //}
    //public bool OnAnimationEnd(string animationStr)
    //{
    //   if (animator.GetBool(animationStr))
    //   {
    //       // animator.fireEvents;
    //        //animationClips[0].events.Length
                
    //       // if (animator.GetCurrentAnimatorStateInfo(0).)
    //        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
    //        //{
    //        //    return true;
    //        //}
    //   }
    //   return false;
    //}
}
