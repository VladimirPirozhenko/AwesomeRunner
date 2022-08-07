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
        playerSM.VerticalDeltaPosition = 0;
        playerSM.HorizontalDeltaPosition = Vector3.zero;
        //Session.ShowGameOverPopUp(true);
        // Session.SetGameOverState();
        // Stats.CalculateScore();
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,0);
    }
    public override void OnStateExit()
    {
        playerSM.PlayDeadAnimation(false);
        // Session.ShowGameOverPopUp(false);
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,1);
    }
    public override void Tick(){}
}
