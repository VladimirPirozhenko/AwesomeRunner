using System.Collections;
using UnityEngine;


public class Pause : MonoBehaviour,ICommandTranslator
{
    private void Start()
    {
        GameSession.Instance.AddCommandTranslator(this);    
    }

    public void TranslateCommand(ECommand command, PressedState state)
    {
        if (state.IsPressed == true)
        {
            switch (command)
            {
                case ECommand.OPEN_PAUSE_MENU:
                    bool isOpened = ViewManager.Instance.IsActive<PausedView>();
                    isOpened = !isOpened;    
                    ViewManager.Instance.Show<PausedView>(isOpened);
                    GameSession.Instance.PauseSession(isOpened);
                    break;
            }
        }
    }
}
