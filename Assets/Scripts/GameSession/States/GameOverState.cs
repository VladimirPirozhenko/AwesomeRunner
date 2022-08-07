using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : GameSessionState
{
    public GameOverState(GameSessionStateMachine gameSessionStateMachine) : base(gameSessionStateMachine)
    {

    }
    public override void OnStateEnter()
    {
        gameSessionSM.ShowGameOverPopUp(true);
    }

    public override void OnStateExit()
    {
        gameSessionSM.ShowGameOverPopUp(false);
    }
    public override void Tick()
    {

    }
}
