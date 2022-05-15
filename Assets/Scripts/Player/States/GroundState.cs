using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MovingState
{
    private PlayerAnimator animator;
    public GroundState(Player player,PlayerController collider, PlayerAnimator animator) : base(player,collider)
    {
        this.animator = animator;
    }
    public override void OnStateEnter()
    {
        animator.SetRunState(true);
    }

    public override void OnStateExit()
    {
        animator.SetRunState(false);
    }

    public override void Tick()
    {
      base.Tick();
    }
}
