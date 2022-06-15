using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSessionState : State<GameSession>
{
    protected GameSessionStateMachine gameSessionSM;
    public GameSessionState(GameSessionStateMachine gameSessionStateMachine)
    {
        gameSessionSM = gameSessionStateMachine;
        //StateMachine = session.SessionStateMachine;
    }
    public override void Tick() { }
}
