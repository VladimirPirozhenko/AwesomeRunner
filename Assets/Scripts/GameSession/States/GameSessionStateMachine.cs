using System.Collections;
using UnityEngine;

public class GameSessionStateMachine : StateMachine<GameSession>
{
    private GameSession session;
    public GameSessionStateMachine(GameSession session)
    {
        this.session = session;
        InitStates();
    }
    #region States 
    public PausedState SessionPausedState { get; private set; }
    public GameOverState SessionGameOverState { get; private set; }
    public ScoreboardState SessionScoreboardState { get; private set; }
    public PlayingState SessionPlayingState { get; private set; }
    private void InitStates()
    {
        SessionPausedState = new PausedState(this);
        SessionGameOverState = new GameOverState(this);
        SessionScoreboardState = new ScoreboardState(this);
        SessionPlayingState = new PlayingState(this);
    }
    #endregion

    public void ShowGameOverPopUp(bool isVisible)
    {
        session.ShowGameOverPopUp(isVisible);   
    }
    public void ShowPauseMenuPopUp(bool isVisible)
    {
        session.ShowPauseMenuPopUp(isVisible);
    }
    public void PauseSession(bool isVisible)
    {
        session.PauseSession(isVisible);
    }
    public void ShowScoreboardUI(bool isVisible)
    {
        session.ShowScoreboardUI(isVisible);
    }
    

}
