using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PausedView : BaseView
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button scoreboardButton;
    [SerializeField] private Button mainMenuButton;
    
    public override void Init()
	{
        resumeButton.onClick.AddListener(() =>
        {
            Show(false);
            GameSession.Instance.PauseSession(false);
            
        });

        scoreboardButton.onClick.AddListener(() =>
        {
            Show(false);
            ViewManager.Instance.Show<ScoreboardView>(true);
        });

        //mainMenuButton.onClick.AddListener(() =>
        //{

        //});

        restartButton.onClick.AddListener(() =>
        {
            GameSession.Instance.RestartSession();
        });
        base.Init();
	}

}
