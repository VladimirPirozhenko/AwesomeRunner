using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSession : MonoBehaviour
{
    //[Serializable] public enum ESessionState { PLAYING, PAUSED_POP_UP, GAMEOVER_POP_UP, SCOREBOARD_UI, MAIN_MENU };
    [SerializeField] private Player player;
    //[SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private Scoreboard scoreboard;
    [SerializeField] private ScoreboardUI scoreboardUI;
    [SerializeField] private GameOverPopUp gameOverPopUp;
    [SerializeField] private PauseMenuPopUp pauseMenuPopUp;

    private GameSessionStateMachine gameSessionStateMachine;
    [SerializeField] public GameSessionState SessionState { get; private set; }

    private void Awake()
    {
        gameSessionStateMachine = new GameSessionStateMachine(this);
       // SetSessionState(gameSessionStateMachine.SessionScoreboardState);
        SetSessionState(gameSessionStateMachine.SessionPlayingState);
        // player = GetComponent<Player>();   
        //SessionStateMachine = new StateMachine<GameSession>();
    }
    private void OnEnable()
    {
        //player.PlayerStatictics.OnScoreCalculated += UpdateScoreboard;
        //player.PlayerHealth.OnOutOfHealth += GameOver;
    }
    private void OnDisable()
    {
        //player.PlayerStatictics.OnScoreCalculated -= UpdateScoreboard;
        //player.PlayerHealth.OnOutOfHealth -= GameOver;
    }
    private void Update()
    {
        //SessionState.Tick();
    //    switch (sessionState)
    //    {
    //        case ESessionState.PLAYING:
    //            break;
    //        case ESessionState.PAUSED:
    //            ShowPauseMenuPopUp(true);
    //            break;
    //        case ESessionState.GAMEOVER:
    //            break;
    //        case ESessionState.SCOREBOARD:
    //            break;
    //    }
    }
    //private void InitStates()
    //{
    //    SessionPausedState = new PausedState(this);
    //    SessionGameOverState = new GameOverState(this);
    //    SessionScoreboardState = new ScoreboardState(this);
    //    SessionPlayingState = new PlayingState(this);
    //}
    public bool IsSessionPaused()
    {
        if (SessionState == gameSessionStateMachine.SessionPausedState)
        {
            return true;
        }
        return false;
    }
    public void GameOver()
    {
        ShowGameOverPopUp(true);
       // SetSessionState(stateComponent.state.GAMEOVER_POP_UP);
    }

    private void SetSessionState(GameSessionState state)//ESessionState state)
    {
        gameSessionStateMachine.SetState(state);
       // sessionState = stateComponent.state;
    }
    public void SetPausedState()
    {
        SetSessionState(gameSessionStateMachine.SessionPausedState);
    }
    public void SetPlayingState()
    {
        SetSessionState(gameSessionStateMachine.SessionPlayingState);
    }
    public void SetGameOverState()
    {
        SetSessionState(gameSessionStateMachine.SessionGameOverState);
    }
    public void SetScoreboardState()
    {
        SetSessionState(gameSessionStateMachine.SessionScoreboardState);
    }
    public void PauseSession(bool isPaused)
    {
        
        if (isPaused)
        {
            Time.timeScale = 0;
            ShowPauseMenuPopUp(true);
           // SetSessionState(ESessionState.PAUSED_POP_UP);
        }
        else
        {
            Time.timeScale = 1;
            ShowPauseMenuPopUp(false);
           // SetSessionState(ESessionState.PLAYING);
        }
        
    }
    public void ClosePopUp()
    {
        //switch (sessionState)
        //{
        //    case ESessionState.PLAYING:
        //        break;
        //    case ESessionState.PAUSED_POP_UP:
        //        ShowPauseMenuPopUp(false);
        //      //  SetSessionState(ESessionState.PLAYING);
        //        break;
        //    case ESessionState.GAMEOVER_POP_UP:
        //        ShowGameOverPopUp(false);
        //       // SetSessionState(ESessionState.PLAYING);
        //        break;
        //    case ESessionState.SCOREBOARD_UI:
        //        ShowScoreboardUI(false);
        //       // SetSessionState(ESessionState.PAUSED_POP_UP);
        //        break;
        //}
    }
    public void HideAllPopUps()
    {
        ShowGameOverPopUp(false);
        ShowPauseMenuPopUp(false);
        ShowScoreboardUI(false);
    }
    public void ShowGameOverPopUp(bool isVisible)
    {
        gameOverPopUp.Show(isVisible);
    }
    public void ShowPauseMenuPopUp(bool isVisible)
    {
        pauseMenuPopUp.Show(isVisible);
    }
    public void ShowScoreboardUI(bool isVisible)
    {
        scoreboardUI.Show(isVisible);
    }

    public void RestartSession()
    {
        //sceneLoader.Load("MainScene"); 
        player.ResetToDefault();
    }
    public void GoToMainMenu()
    {
        //sceneLoader.Load("MainMenu");
        player.ResetToDefault();
    }
    public void AddEntryToScoreboard(ScoreboardEntry entry)
    {
        scoreboard.AddScoreboardEntry(entry);
        //scoreboard.gameObject.SetActive(true);
    }
    public void SaveSessionResult()
    {
        scoreboard.SaveScoreboardEntriesTable();    
    }
    public void UpdateScoreboard(int score)
    {
        AddEntryToScoreboard(new ScoreboardEntry("PlayerOne", score));
        SaveSessionResult();
    }
}
