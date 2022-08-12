using System.Collections;
using UnityEngine;


public class Pause : MonoBehaviour,ICommandTranslator
{
    private bool isOpened = false;
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
                    isOpened = isOpened ? false : true;
                    ViewManager.Instance.Show<PausedView>(isOpened);
                    GameSession.Instance.PauseSession(isOpened);
                    break;
            }
        }
    }
}
