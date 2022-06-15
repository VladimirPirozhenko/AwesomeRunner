using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardState : PausedState
{
    public ScoreboardState(GameSessionStateMachine gameSessionStateMachine) : base(gameSessionStateMachine)
    {

    }
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        gameSessionSM.ShowPauseMenuPopUp(false);
        // session.scoreboardUI.Show(true);
        gameSessionSM.ShowScoreboardUI(true);
    }
    public override void OnStateExit()
    {
        gameSessionSM.ShowScoreboardUI(false);
        gameSessionSM.ShowPauseMenuPopUp(true);
        // base.OnStateExit();
    }
    public override void Tick()
    {

    }
}
