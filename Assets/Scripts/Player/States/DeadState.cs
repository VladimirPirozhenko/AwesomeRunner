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
        playerSM.PlayerTransform.position = new Vector3(playerSM.PlayerTransform.position.x,0.38f, playerSM.PlayerTransform.position.z);
        playerSM.VerticalDeltaPosition = 0;
        playerSM.HorizontalDeltaPosition = Vector3.zero;
        GameSession.Instance.RestrictInputs(InputConstants.InGameCommands, true);
       
        //Session.ShowGameOverPopUp(true);
        // Session.SetGameOverState();
        // Stats.CalculateScore();
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,0);
    }
    public override void OnStateExit()
    {
        playerSM.PlayDeadAnimation(false);
        GameSession.Instance.RestrictInputs(InputConstants.InGameCommands, false);
        // Session.ShowGameOverPopUp(false);
        //playerSM.ChangeRigWeight(playerSM.RightHandRig,1);
    }
    public override void Tick(){}
}
