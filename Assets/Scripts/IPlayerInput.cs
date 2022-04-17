using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum Direction { LEFT,RIGHT };
public interface IPlayerInput 
{
    public float Horizontal { get; }
    public float Vertical { get; }
    public void ReadInput();
}
