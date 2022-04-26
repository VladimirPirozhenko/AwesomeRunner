using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowKeysInput : IPlayerInput //ядекюрэ лнмнаеу х явхршбюрэ хмоср онярнъммн б юодеир х ядекюрэ церхмоср
{
    EDirection? IPlayerInput.ScanDirection()
    {
        //float Horizontal = Input.GetAxisRaw("Horizontal");
        //float Vertical = Input.GetAxisRaw("Vertical");
        //if (Horizontal == 1)
        //    return EDirection.RIGHT;
        //if (Horizontal == -1)
        //    return EDirection.LEFT;
        //if (Vertical == 1)
        //    return EDirection.UP;
        //if (Vertical == -1)
        //    return EDirection.DOWN;
        //else
        //    return null;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            return EDirection.UP;
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            return EDirection.DOWN;
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            return EDirection.RIGHT;
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            return EDirection.LEFT;
        else
            return null;
    }
}
