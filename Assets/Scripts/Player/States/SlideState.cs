using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : MovingState
{
    private PlayerAnimator animator;
    private float expiredTime = 0;
    private float slideDuration = 1;
    public SlideState(Player player, PlayerController collider,PlayerAnimator animator) : base(player, collider)
    {
        this.animator = animator;
    }
    public override void OnStateEnter()
    {
        animator.SetSlideState(true);
        player.PlayerController.ChangeColliderCenter(player.PlayerController.defaultCenter / 2);
        player.PlayerController.ChangeColliderHeight(player.PlayerController.defaultHeight / 2);
    }
    public override void OnStateExit()
    {
        player.VerticalDeltaPosition = 0;
        expiredTime = 0;
        player.PlayerController.ResetToDefault();
        animator.SetSlideState(false);
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
        if (slideProgress > 1)
        {
            expiredTime = 0;
            player.PlayerStateMachine.SetState(player.PlayerGroundState);
        }
    }
}
