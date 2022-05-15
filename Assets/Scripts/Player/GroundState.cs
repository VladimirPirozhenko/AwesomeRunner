using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MovingState
{
    private PlayerAnimator animator;
    public GroundState(Player player,PlayerController collider, PlayerAnimator animator) : base(player,collider)
    {
        //this.player = player;
        this.animator = animator;
    }
    public override void OnStateEnter()
    {
        animator.SetRunState(true);
        //throw new System.NotImplementedException();
    }

    public override void OnStateExit()
    {
        animator.SetRunState(false);
        // throw new System.NotImplementedException();
    }

    public override void Tick()
    {
      base.Tick();
    }
}
