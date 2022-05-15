using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StartingIdleState : State<Player>
{
    private Player player;
    private PlayerAnimator animator;
    private int countdownTime;
    public StartingIdleState(Player player, PlayerAnimator animator)
    {
        this.player = player;
        this.animator = animator;
    }
    public override void OnStateEnter()
    {
        animator.SetIdleState(true);
        player.PlayerStatictics.ShowGameOverPopUp(false);
        player.transform.position = new Vector3(0, 0, 0);
        countdownTime = 3;
        CountdownBeforeTheStart();
    }
    public override void OnStateExit()
    {
        animator.SetIdleState(false);
        player.HorizontalDeltaPosition = Vector3.zero;
        player.VerticalDeltaPosition = 0f;
    }
    public override void Tick()
    {
       
    }
    public async void CountdownBeforeTheStart()
    {
        while (countdownTime > 0)
        {     
            countdownTime--;
            await Task.Delay(1000);
        }
        player.PlayerStateMachine.SetState(player.PlayerGroundState);
    }
}
