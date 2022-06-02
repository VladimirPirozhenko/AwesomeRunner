using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : MovingState
{
    public GroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {}
    public override void OnStateEnter()
    {
        playerSM.PlayRunningAnimation(true);
    }

    public override void OnStateExit()
    {
        playerSM.PlayRunningAnimation(false);
    }

    public override void Tick()
    {
      base.Tick();
    }
}
