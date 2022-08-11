using System.Collections;
using UnityEngine;


public class GameSession : MonoBehaviour
{
    public InputTranslator<KeyBinding> InputTranslator;

    public static GameSession Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        InputTranslator = new InputTranslator<KeyBinding>();
        IBindingHolder<KeyBinding> holder = new KeyBindingHolder();
        InputTranslator.Init(holder);
    }

    private void Update()
    {
        InputTranslator.Tick();
    }
}