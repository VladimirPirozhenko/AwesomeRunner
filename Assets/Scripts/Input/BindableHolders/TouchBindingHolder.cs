using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchBindingHolder : IBindingHolder<TouchBinding>
{
    public Dictionary<ECommand, TouchBinding> InputBindings { get; private set; }

    private readonly Dictionary<ECommand, TouchBinding> DefaultTouchBindings = new Dictionary<ECommand, TouchBinding>
    {
        {ECommand.NONE, new TouchBinding(ETouchGesture.NONE)},
        {ECommand.DOWN, new TouchBinding(ETouchGesture.SWIPE_DOWN)},
        {ECommand.UP, new TouchBinding(ETouchGesture.SWIPE_UP)},
        {ECommand.LEFT, new TouchBinding(ETouchGesture.SWIPE_LEFT)},
        {ECommand.RIGHT, new TouchBinding(ETouchGesture.SWIPE_RIGHT)},
    };

    public void Init()
    {
        if (InputBindings == null)
            InputBindings = DefaultTouchBindings;
    }
}
