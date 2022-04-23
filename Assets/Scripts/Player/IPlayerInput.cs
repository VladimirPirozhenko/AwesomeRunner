using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDirection { LEFT, RIGHT , UP ,DOWN };
public interface IPlayerInput 
{
    public EDirection? ScanDirection();
}
