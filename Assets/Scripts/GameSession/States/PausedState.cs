using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedState : GameSessionState
{
    public PausedState(GameSessionStateMachine gameSessionStateMachine) : base(gameSessionStateMachine)
    {

    }
    public override void OnStateEnter()
    {
        //gameSessionSM.ShowPauseMenuPopUp(true);
        gameSessionSM.PauseSession(true);
    }

    public override void OnStateExit()
    {
       // gameSessionSM.ShowPauseMenuPopUp(false);
        gameSessionSM.PauseSession(false);
    }
    public override void Tick()
    {

    }
}
