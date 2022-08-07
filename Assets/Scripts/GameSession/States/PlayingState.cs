using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : GameSessionState
{
    public PlayingState(GameSessionStateMachine gameSessionStateMachine) : base(gameSessionStateMachine)
    {

    }
    public override void OnStateEnter()
    {
        gameSessionSM.ShowPauseMenuPopUp(false);
        gameSessionSM.PauseSession(false);
    }
    public override void OnStateExit()
    {

    }
    public override void Tick()
    {

    }
}
