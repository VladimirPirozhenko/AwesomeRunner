using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PausedView : BaseView
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    public override void Init()
	{
        resumeButton.onClick.AddListener(() =>
        {
            GameSession.Instance.PauseSession(false);
            Show(false);
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
