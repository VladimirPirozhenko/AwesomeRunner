using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputConstants
{
    public static readonly List<ECommand> InGameCommands = new List<ECommand> { ECommand.LEFT, ECommand.RIGHT, ECommand.UP,ECommand.DOWN, ECommand.SHOOT };
    public static readonly List<ECommand> ViewCommands = new List<ECommand> { ECommand.OPEN_SCOREBOARD, ECommand.OPEN_PAUSE_MENU };
}