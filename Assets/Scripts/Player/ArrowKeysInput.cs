using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowKeysInput : IPlayerInput //ядекюрэ лнмнаеу х явхршбюрэ хмоср онярнъммн б юодеир, ядекюрэ церхмоср?
{
    public bool IsShooting()
    {
        if (Input.GetKeyDown(KeyCode.F))
            return true;
        return false;
    }
    EInputDirection? IPlayerInput.ScanDirection()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            return EInputDirection.UP;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            return EInputDirection.DOWN;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            return EInputDirection.RIGHT;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            return EInputDirection.LEFT;
        else
            return null;
    }


}
