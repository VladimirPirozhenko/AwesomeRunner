using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowKeysInput : IPlayerInput
{
    EDirection? IPlayerInput.ScanDirection()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            return EDirection.UP;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            return EDirection.DOWN;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            return EDirection.RIGHT;
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            return EDirection.LEFT;
        else
            return null;
    }
}
