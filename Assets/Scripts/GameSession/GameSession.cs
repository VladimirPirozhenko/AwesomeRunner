using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour,IResettable
{
    private InputTranslator<KeyBinding> InputTranslator;

    public static GameSession Instance { get; private set; }

    [SerializeField] private Player currentPlayer;

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
        Time.timeScale = isPaused ?  0 : 1;
        List<ECommand> commands = new List<ECommand>();
        ECommand[] commandRange = { ECommand.LEFT,ECommand.RIGHT,ECommand.UP,ECommand.DOWN,ECommand.SHOOT};
        commands.AddRange(commandRange);
        InputTranslator.RestictTranslation(commands,isPaused);
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