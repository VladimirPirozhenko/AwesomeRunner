using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyBindingHolder : IBindingHolder<KeyBinding>
{
    public Dictionary<ECommand, KeyBinding> InputBindings { get; private set; }

    private readonly Dictionary<ECommand, KeyBinding> DefaultKeyBindings = new Dictionary<ECommand, KeyBinding>
    {
        {ECommand.NONE, new KeyBinding(KeyCode.None)},
        {ECommand.DOWN, new KeyBinding(KeyCode.DownArrow,KeyCode.S)},
        {ECommand.UP, new KeyBinding(KeyCode.UpArrow,KeyCode.W)},
        {ECommand.LEFT, new KeyBinding(KeyCode.LeftArrow,KeyCode.A)},
        {ECommand.RIGHT, new KeyBinding(KeyCode.RightArrow,KeyCode.D)},
        {ECommand.OPEN_SCOREBOARD, new KeyBinding(KeyCode.Tab)},
        {ECommand.OPEN_PAUSE_MENU, new KeyBinding(KeyCode.Escape)}
    };

    public void Init()
    {
        if (InputBindings == null)
            InputBindings = DefaultKeyBindings;
    }
}
