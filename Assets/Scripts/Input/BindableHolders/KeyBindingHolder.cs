using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyBindingHolder : IBindingHolder<KeyBinding>
{
    public Dictionary<ECommand, KeyBinding> InputBindings { get; private set; }

    private readonly Dictionary<ECommand, KeyBinding> DefaultKeyBindings = new Dictionary<ECommand, KeyBinding>
    {
        {ECommand.DOWN, new KeyBinding(KeyCode.DownArrow)},
        {ECommand.UP, new KeyBinding(KeyCode.UpArrow)},
        {ECommand.LEFT, new KeyBinding(KeyCode.LeftArrow)},
        {ECommand.RIGHT, new KeyBinding(KeyCode.RightArrow)},
    };

    public void Init()
    {
        if (InputBindings == null)
            InputBindings = DefaultKeyBindings;
    }
}
