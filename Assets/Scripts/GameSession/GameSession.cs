using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour,IResettable
{
    public static GameSession Instance { get; private set; } 

    [SerializeField] private Player currentPlayer;

    private InputTranslator<KeyBinding> InputTranslator;

    private bool isSessionPaused = false;
    private bool isInputAlreadyRestricted = false;
    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Update()
    {
       InputTranslator.Tick();
    }

    private void Init()
    {
        InputTranslator = new InputTranslator<KeyBinding>();
        IBindingHolder<KeyBinding> holder = new KeyBindingHolder();
        InputTranslator.Init(holder);
    }

    public void AddCommandTranslator(ICommandTranslator translator)
    {
        InputTranslator.AddCommandTranslator(translator);   
    }

  
    public void PauseSession(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        if (!isSessionPaused && InputTranslator.IsTranslationResticted(InputConstants.InGameCommands))
        {
            isInputAlreadyRestricted = true;
            isSessionPaused = isPaused;
            return;
        }
        if (!InputTranslator.IsTranslationResticted(InputConstants.InGameCommands))
        {
            isInputAlreadyRestricted = false;
        }
        isSessionPaused = isPaused;
        if (isInputAlreadyRestricted)
        {
            return;
        }   
        RestrictInputs(InputConstants.InGameCommands,isRestricted: isPaused);
    }

    public void RestrictInputs(List<ECommand> commands,bool isRestricted)
    {
        InputTranslator.RestictTranslation(commands, isRestricted);
    }
    public void RestartSession()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        ResetToDefault();
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        ResetToDefault();
    }

    public void ResetToDefault()
    {
        PauseSession(false);
        currentPlayer.ResetToDefault();
    }
}