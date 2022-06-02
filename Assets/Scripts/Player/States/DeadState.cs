using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : PlayerState
{
    public DeadState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {}
    public override void OnStateEnter()
    {
        playerSM.PlayDeadAnimation(true);
        VerticalDeltaPosition = 0;
        HorizontalDeltaPosition = Vector3.zero;
        //Session.ShowGameOverPopUp(true);
        // Session.SetGameOverState();
        // Stats.CalculateScore();
        playerSM.ChangeRightHandRigWeight(0);
    }
    public override void OnStateExit()
    {
        playerSM.PlayDeadAnimation(false);
        // Session.ShowGameOverPopUp(false);
        playerSM.ChangeRightHandRigWeight(1);
    }
    public override void Tick(){}
}
