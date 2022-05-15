using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<Player>
{
    private Player player;
    PlayerAnimator animator;
    public DeadState(Player player, PlayerAnimator animator) 
    {
        this.player = player;
        this.animator = animator;   
    }
    public override void OnStateEnter()
    {
        animator.SetDeadState(true);
        player.VerticalDeltaPosition = 0;
        player.HorizontalDeltaPosition = Vector3.zero;
        player.PlayerStatictics.ShowGameOverPopUp(true);

    }
    public override void OnStateExit()
    {
        animator.SetDeadState(false);
    }
    public override void Tick(){}
}
